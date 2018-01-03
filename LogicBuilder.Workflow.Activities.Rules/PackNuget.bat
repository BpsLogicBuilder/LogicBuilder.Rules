REM To Use update the version number and call: PackNuget
SET "version=1.0.0-preview05"
CD /d %0\..
msbuild /t:pack /p:Configuration=Release
nuget add bin\Release\LogicBuilder.Workflow.Activities.Rules.%version%.nupkg -source C:\LocalNuget\packages
REM nuget push bin\Release\LogicBuilder.Workflow.Activities.Rules.%version%.nupkg -Source https://www.nuget.org/api/v2/package
pause
