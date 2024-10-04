$scriptName = $MyInvocation.MyCommand.Name

Write-Host "Owner ${Env:REPO_OWNER}"
Write-Host "Repository ${Env:REPO}"

$PROJECT_PATH = ".\$($Env:PROJECT_NAME)\$($Env:PROJECT_NAME).csproj"
$NUGET_PACKAGE_PATH = ".\artifacts\$($Env:PROJECT_NAME).*.nupkg"

Write-Host "Project Path ${PROJECT_PATH}"
Write-Host "Package Path ${NUGET_PACKAGE_PATH}"

msbuild $PROJECT_PATH -t:restore
msbuild $PROJECT_PATH /p:Configuration=Release

if ($Env:REPO_OWNER -ne "BpsLogicBuilder") {
    Write-Host "${scriptName}: Only create packages on BpsLogicBuilder repositories."
} else {
    nuget pack $PROJECT_PATH -properties Configuration=Release -OutputDirectory .\artifacts
    dotnet nuget push $NUGET_PACKAGE_PATH --skip-duplicate --api-key $Env:GITHUB_NUGET_AUTH_TOKEN
}