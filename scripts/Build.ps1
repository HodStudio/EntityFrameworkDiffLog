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

if(Test-Path .\artifacts) { Remove-Item .\artifacts -Force -Recurse }

exec { & dotnet restore }

exec { & dotnet build $projectPath -c Release }

$csprojContent = Get-Content $projectPath
$regexSuffix = "<VersionSuffix>(.+)<\/VersionSuffix>"
$projSuffix = $csprojContent | Select-String -Pattern $regexSuffix | % { "$($_.matches.groups[1])" }
$revision = $NULL

if ($projSuffix -ne $NULL)
{
	echo "Project sufix: $($projSuffix)"
	$revision = @{ $true = $env:APPVEYOR_BUILD_NUMBER; $false = 1 }[$env:APPVEYOR_BUILD_NUMBER -ne $NULL];
	$revision = "$projSuffix-{0:D2}" -f [convert]::ToInt32($revision, 10)
}

# exec { & dotnet test .\test\HodStudio.EfDiffLog.Tests -c Release }

exec { & dotnet pack .\src\HodStudio.EfDiffLog\HodStudio.EfDiffLog.csproj -c Release -o .\..\..\artifacts --version-suffix=$revision }