REM To Use update the version number and call: PackNuget
SET "version=1.0.0-preview06"
CD /d %0\..
msbuild /t:pack /p:Configuration=Release
nuget add bin\Release\LogicBuilder.ComponentModel.Design.Serialization.%version%.nupkg -source C:\LocalNuget\packages
REM nuget push bin\Release\LogicBuilder.ComponentModel.Design.Serialization.%version%.nupkg -Source https://www.nuget.org/api/v2/package
pause
