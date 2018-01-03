REM To Use update the version number and call: PackNuget
SET "version=1.0.0-preview05"
CD /d %0\..
msbuild /t:pack /p:Configuration=Release
nuget add bin\Release\LogicBuilder.ComponentModel.Design.Serialization.%version%.nupkg -source C:\LocalNuget\packages
pause
