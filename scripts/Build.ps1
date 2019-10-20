$projectPath = ".\src\HodStudio.EfDiffLog\HodStudio.EfDiffLog.csproj"

if(Test-Path .\artifacts) { Remove-Item .\artifacts -Force -Recurse }

exec { & dotnet restore }

exec { & dotnet build $projectPath -c Release }

$revision = @{ $true = $env:APPVEYOR_BUILD_NUMBER; $false = 1 }[$env:APPVEYOR_BUILD_NUMBER -ne $NULL];
$revision = "{0:D4}" -f [convert]::ToInt32($revision, 10)

# exec { & dotnet test .\test\HodStudio.EfDiffLog.Tests -c Release }

exec { & dotnet pack .\src\HodStudio.EfDiffLog\HodStudio.EfDiffLog.csproj -c Release -o .\artifacts --version-suffix=$revision }