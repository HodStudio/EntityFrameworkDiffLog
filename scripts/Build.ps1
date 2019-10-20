# Taken from psake https://github.com/psake/psake

<#
.SYNOPSIS
  This is a helper function that runs a scriptblock and checks the PS variable $lastexitcode
  to see if an error occcured. If an error is detected then an exception is thrown.
  This function allows you to run command-line programs without having to
  explicitly check the $lastexitcode variable.
.EXAMPLE
  exec { svn info $repository_trunk } "Error executing SVN. Please verify SVN command-line client is installed"
#>
function Exec
{
    [CmdletBinding()]
    param(
        [Parameter(Position=0,Mandatory=1)][scriptblock]$cmd,
        [Parameter(Position=1,Mandatory=0)][string]$errorMessage = ($msgs.error_bad_command -f $cmd)
    )
    & $cmd
    if ($lastexitcode -ne 0) {
        throw ("Exec: " + $errorMessage)
    }
}

$projectPath = ".\src\HodStudio.EfDiffLog\HodStudio.EfDiffLog.csproj"
$solutionPath = ".\HodStudio.EfDiffLog.LibraryOnly.sln"

if(Test-Path .\artifacts) { Remove-Item .\artifacts -Force -Recurse }

# Version Configuration
$csprojContent = Get-Content $projectPath
$regexSuffix = "<VersionSuffix>(.+)<\/VersionSuffix>"
$regexPrefix = "<VersionPrefix>(.+)<\/VersionPrefix>"
$projSuffix = $csprojContent | Select-String -Pattern $regexSuffix | % { "$($_.matches.groups[1])" }
$projPrefix = $csprojContent | Select-String -Pattern $regexPrefix | % { "$($_.matches.groups[1])" }
$revision = $NULL

if ($projSuffix -ne $NULL)
{
	echo "Project sufix: $($projSuffix)"
	$revision = @{ $true = $env:APPVEYOR_BUILD_NUMBER; $false = 1 }[$env:APPVEYOR_BUILD_NUMBER -ne $NULL];
	$revision = "$projSuffix-{0:D2}" -f [convert]::ToInt32($revision, 10)
}
$completeVersion = "$($projPrefix)-$($revision)"

# Restore packages
exec { & dotnet restore $solutionPath }

# Sonar Analysis
Try
{
    echo "Installing sonarscanner"
    exec { & dotnet tool install -g dotnet-sonarscanner }
}
Catch
{
    $needInstallSonar = dotnet tool list -g | Select-String -Pattern "dotnet-sonarscanner" | % { $_.Matches.Value -eq $NULL }
    if ($needInstallSonar -eq $true)
    {
        $ErrorMessage = $_.Exception.Message
        $FailedItem = $_.Exception.ItemName
        echo $FailedItem
        echo $ErrorMessage
        return -1
    }
    else { echo "sonarscanner already installed" }
}

exec { & dotnet sonarscanner begin /d:sonar.login=%sonartoken% /key:"HodStudio.EfDiffLog" /o:"hodstudio-github" /d:sonar.sources=".\src\HodStudio.EfDiffLog" /d:sonar.host.url="https://sonarcloud.io" /version:"$completeVersion" }

exec { & dotnet build $solutionPath -c Release }

exec { & dotnet sonarscanner end /d:sonar.login=%sonartoken% }

# exec { & dotnet test .\test\HodStudio.EfDiffLog.Tests -c Release }

exec { & dotnet pack $projectPath -c Release -o .\artifacts --version-suffix=$revision }