$VERSION = "1.0.3-preview01"
$SOLUTION_PATH = "C:\.NetStandardGit\LogicBuilder\LogicBuilder.Rules"

function Push 
{
	param([string]$PROJECT)

	dotnet build "$SOLUTION_PATH\$($PROJECT)\$($PROJECT).csproj" --configuration Release
	msbuild /t:pack "$SOLUTION_PATH\$($PROJECT)\$($PROJECT).csproj" /p:Configuration=Release
	nuget add "$SOLUTION_PATH\$($PROJECT)\bin\Release\$($PROJECT).$($VERSION).nupkg" -source C:\LocalNuget\packages
}

Push "LogicBuilder.ComponentModel.Design.Serialization"
Push "LogicBuilder.Workflow.ComponentModel.Serialization"
Push "LogicBuilder.Workflow.Activities.Rules"