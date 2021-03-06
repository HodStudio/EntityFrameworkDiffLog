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

$projectPath = "$($env:APPVEYOR_BUILD_FOLDER)\src\HodStudio.EntityFrameworkDiffLog\HodStudio.EntityFrameworkDiffLog.csproj"
$libraryOnlySolutionPath = "$($env:APPVEYOR_BUILD_FOLDER)\src\HodStudio.EntityFrameworkDiffLog\HodStudio.EntityFrameworkDiffLog.Library.sln"

if(Test-Path .\artifacts) { Remove-Item .\artifacts -Force -Recurse }
if(Test-Path .\testresults) { Remove-Item .\testresults -Force -Recurse }

# Version Configuration
$csprojContent = Get-Content $projectPath
$regexSuffix = "<VersionSuffix>(.+)<\/VersionSuffix>"
$regexPrefix = "<VersionPrefix>(.+)<\/VersionPrefix>"
$projSuffix = $csprojContent | Select-String -Pattern $regexSuffix | % { "$($_.matches.groups[1])" }
$projPrefix = $csprojContent | Select-String -Pattern $regexPrefix | % { "$($_.matches.groups[1])" }
$revision = $NULL
$completeVersionSuffix = $NULL

if ($projSuffix -ne $NULL)
{
	echo "Project sufix: $($projSuffix)"
	$revision = @{ $true = $env:APPVEYOR_BUILD_NUMBER; $false = 1 }[$env:APPVEYOR_BUILD_NUMBER -ne $NULL];
	$revision = "$projSuffix-{0:D2}" -f [convert]::ToInt32($revision, 10)
	$completeVersionSuffix = "-$($revision)"
}
$completeVersion = "$($projPrefix)$($completeVersionSuffix)"
echo "Complete project version: $($completeVersion)"

# Restore packages
echo "Restoring packages"
exec { & dotnet restore }
exec { & .\tools\NuGet.exe restore .\src\HodStudio.EntityFrameworkDiffLog.TestsDotNet45\HodStudio.EntityFrameworkDiffLog.TestsDotNet45.csproj -PackagesDirectory .\packages }

echo "Build entire solution"
exec { & dotnet build -c Release }

echo "Tests Core version"
dotnet test -c Release --test-adapter-path:. --logger:"nunit;LogFilePath=$($env:APPVEYOR_BUILD_FOLDER)\TestResults\core-results.xml" -s "$($env:APPVEYOR_BUILD_FOLDER)\coverletArgs.runsettings" -r "$($env:APPVEYOR_BUILD_FOLDER)\TestResults\"

$corePassed = $lastexitcode

echo "Adjust Configuration for Tests 4.5"
Remove-Item -Path "$($env:APPVEYOR_BUILD_FOLDER)\src\HodStudio.EntityFrameworkDiffLog.TestsDotNet45\bin\Release\HodStudio.EntityFrameworkDiffLog.TestsDotNet45.dll.config"
Rename-Item -Path "$($env:APPVEYOR_BUILD_FOLDER)\src\HodStudio.EntityFrameworkDiffLog.TestsDotNet45\bin\Release\App.Release.config" -NewName "HodStudio.EntityFrameworkDiffLog.TestsDotNet45.dll.config"

echo "Tests 4.5 version"
.\packages\NUnit.ConsoleRunner.3.10.0\tools\nunit3-console.exe .\src\HodStudio.EntityFrameworkDiffLog.TestsDotNet45\bin\Release\HodStudio.EntityFrameworkDiffLog.TestsDotNet45.dll --result="$($env:APPVEYOR_BUILD_FOLDER)\TestResults\net45-results.xml"

$net45Passed = $lastexitcode

echo "Upload results to AppVeyor"
$wc = New-Object 'System.Net.WebClient'
$wc.UploadFile("https://ci.appveyor.com/api/testresults/nunit3/$($env:APPVEYOR_JOB_ID)", (Resolve-Path "$($env:APPVEYOR_BUILD_FOLDER)\TestResults\net45-results.xml" ))
$wc.UploadFile("https://ci.appveyor.com/api/testresults/nunit3/$($env:APPVEYOR_JOB_ID)", (Resolve-Path "$($env:APPVEYOR_BUILD_FOLDER)\TestResults\core-results.xml" ))

$errorMessage = $NULL

if ($corePassed -ne 0)
{
	$errorMessage = ".Net Core tests failed`r`n"
}

if ($net45Passed -ne 0)
{
    $errorMessage = $errorMessage + ".Net 4.5 tests failed`r`n"
}

if ($errorMessage -ne $NULL)
{
    throw $errorMessage
}

# Sonar Analysis
echo "Installing sonarscanner"
Try
{
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

echo "Starting Sonar for Library"

if ($env:APPVEYOR_PULL_REQUEST_NUMBER -ne $null)
{
	exec { & dotnet sonarscanner begin `
	/d:sonar.login="$env:sonartoken" `
	/key:"HodStudio.EntityFrameworkDiffLog" `
	/o:"hodstudio-github" `
	/d:sonar.sources=".\src\HodStudio.EntityFrameworkDiffLog" `
	/d:sonar.host.url="https://sonarcloud.io" `
	/d:sonar.pullrequest.base="$env:APPVEYOR_REPO_BRANCH" `
	/d:sonar.pullrequest.branch="$env:APPVEYOR_PULL_REQUEST_HEAD_REPO_BRANCH" `
	/d:sonar.pullrequest.key="$env:APPVEYOR_PULL_REQUEST_NUMBER " `
	/d:sonar.pullrequest.provider="GitHub" `
	/d:sonar.pullrequest.github.repository="$env:APPVEYOR_REPO_NAME" `
	}
}
else 
{
	exec { & dotnet sonarscanner begin `
	/d:sonar.login="$env:sonartoken" `
	/key:"HodStudio.EntityFrameworkDiffLog" `
	/o:"hodstudio-github" `
	/d:sonar.sources=".\src\HodStudio.EntityFrameworkDiffLog" `
	/d:sonar.host.url="https://sonarcloud.io" `
	/version:"$completeVersion" `
	}
}

exec { & dotnet build $libraryOnlySolutionPath -c Release }

exec { & dotnet sonarscanner end /d:sonar.login="$env:sonartoken" }

echo "Code Coverage information"

$opencoverFile = Get-ChildItem -Path "$($env:APPVEYOR_BUILD_FOLDER)\TestResults" -Filter coverage.opencover.xml -Recurse -ErrorAction SilentlyContinue -Force

Copy-Item $opencoverFile.FullName -Destination "$($env:APPVEYOR_BUILD_FOLDER)\TestResults"

echo "Installing coverralls"
Try
{
    exec { & dotnet tool install -g coveralls.net }
}
Catch
{
    $needInstallSonar = dotnet tool list -g | Select-String -Pattern "coveralls.net" | % { $_.Matches.Value -eq $NULL }
    if ($needInstallSonar -eq $true)
    {
        $ErrorMessage = $_.Exception.Message
        $FailedItem = $_.Exception.ItemName
        echo $FailedItem
        echo $ErrorMessage
        return -1
    }
    else { echo "coveralls already installed" }
}
if ($env:APPVEYOR_PULL_REQUEST_NUMBER -ne $null)
{
	exec { & csmacnz.Coveralls `
	--opencover `
	-i "$($env:APPVEYOR_BUILD_FOLDER)\TestResults\coverage.opencover.xml" `
	--repoToken $env:coverallstoken `
	--useRelativePaths `
	--commitId $env:APPVEYOR_REPO_COMMIT `
	--commitBranch $env:APPVEYOR_REPO_BRANCH `
	--commitAuthor $env:APPVEYOR_REPO_COMMIT_AUTHOR `
	--commitEmail $env:APPVEYOR_REPO_COMMIT_AUTHOR_EMAIL `
	--commitMessage $env:APPVEYOR_REPO_COMMIT_MESSAGE `
	--jobId $env:APPVEYOR_BUILD_NUMBER `
	--serviceName appveyor `
	--pullRequest $env:APPVEYOR_PULL_REQUEST_NUMBER
	}
}
else {
	exec { & csmacnz.Coveralls `
	--opencover `
	-i "$($env:APPVEYOR_BUILD_FOLDER)\TestResults\coverage.opencover.xml" `
	--repoToken $env:coverallstoken `
	--useRelativePaths `
	--commitId $env:APPVEYOR_REPO_COMMIT `
	--commitBranch $env:APPVEYOR_REPO_BRANCH `
	--commitAuthor $env:APPVEYOR_REPO_COMMIT_AUTHOR `
	--commitEmail $env:APPVEYOR_REPO_COMMIT_AUTHOR_EMAIL `
	--commitMessage $env:APPVEYOR_REPO_COMMIT_MESSAGE `
	--jobId $env:APPVEYOR_BUILD_NUMBER `
	--serviceName appveyor 
	}
}

echo "Packing the library"
exec { & dotnet pack $projectPath -c Release -o "$($env:APPVEYOR_BUILD_FOLDER)\artifacts" --version-suffix=$revision }