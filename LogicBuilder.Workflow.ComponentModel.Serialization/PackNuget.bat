REM To Use update the version number and call: PackNuget
SET "version=1.0.2"
CD /d %0\..
msbuild /t:pack /p:Configuration=Release
nuget add bin\Release\LogicBuilder.Workflow.ComponentModel.Serialization.%version%.nupkg -source C:\LocalNuget\packages
REM nuget push bin\Release\LogicBuilder.Workflow.ComponentModel.Serialization.%version%.nupkg -Source https://www.nuget.org/api/v2/package
pause
