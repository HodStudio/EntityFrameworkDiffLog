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

$projectPath = ".\src\HodStudio.EntityFrameworkDiffLog\HodStudio.EntityFrameworkDiffLog.csproj"
$libraryOnlySolutionPath = ".\src\HodStudio.EntityFrameworkDiffLog\HodStudio.EntityFrameworkDiffLog.Library.sln"

if(Test-Path .\artifacts) { Remove-Item .\artifacts -Force -Recurse }
if(Test-Path .\testresults) { Remove-Item .\testresults -Force -Recurse }

# Restore packages
echo "Restoring packages"
exec { & dotnet restore }

echo "Build entire solution"
exec { & dotnet build -c Release }

echo "Tests Core version"
dotnet test -c Release -s coverletArgs.runsettings -r ".\TestResults\"

echo "Installing reportgenerator"
Try
{
    exec { & dotnet tool install -g dotnet-reportgenerator-globaltool }
}
Catch
{
    $needInstallSonar = dotnet tool list -g | Select-String -Pattern "dotnet-reportgenerator-globaltool" | % { $_.Matches.Value -eq $NULL }
    if ($needInstallSonar -eq $true)
    {
        $ErrorMessage = $_.Exception.Message
        $FailedItem = $_.Exception.ItemName
        echo $FailedItem
        echo $ErrorMessage
        return -1
    }
    else { echo "reportgenerator already installed" }
}

exec { & reportgenerator "-reports:TestResults\*\*.xml" "-targetdir:TestResults\" "-reporttypes:Badges;Html;SonarQube" }