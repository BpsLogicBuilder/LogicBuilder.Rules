REM To Use update the version number and call: PackNuget
SET "version=1.0.0"
CD /d %0\..
nuget pack LogicBuilder.Workflow.Activities.Rules.Design.csproj -properties Configuration=Release
nuget add LogicBuilder.Workflow.Activities.Rules.Design.%version%.nupkg -source C:\LocalNuget\packages
REM nuget push LogicBuilder.Workflow.Activities.Rules.Design.%version%.nupkg -Source https://www.nuget.org/api/v2/package
pause
