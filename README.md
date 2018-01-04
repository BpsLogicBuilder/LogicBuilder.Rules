# LogicBuilder.Rules
This project removes the deprecated sections from System.Workflow.Activities.Rules, makes the assembly compatible with .NetStandard 2.0. and updates the namespace to LogicBuilder.Workflow.Activities.Rules.

## To get started:
* Build the solution.

## Run the unit tests
The unit tests (one set of tests each for .Net Core and the .Net Framework) create rulesets using CodeDom expressions, execute the rules engine, and evaluate the results.

## Create serialized rule sets
* Execute Console.NetCore.DefineAndSerialize and Console.NetFramework.DefineAndSerialize.  Each console application creates and serializes a rule set for the corresponding platform (.Net Core and .Net Framework respectively).
* Check the "ruleSetFile" entry in the settings files (appSettings.json for .Net Core and Settings.settings for the .NetFramework) for the update locations.

## Rule Set Editor
* Set the start up project to RuleSetToolkit and run the winforms application.
* To use a rule set created by Console.NetCore.DefineAndSerialize or Console.NetFramework.DefineAndSerialize, set:
  * Activity Class: SampleFlow.FlowEntity
  * Activity Assembly: (Full path to SampleFlow.dll) (SampleFlow is a project in the solution)
  * Ruleset: (Full Path to either file serialized by Console.NetCore.DefineAndSerialize or Console.NetFramework.DefineAndSerialize) (defaults are: C:\Temp\FlowEntity.NetCore.module and C:\Temp\FlowEntity.NetFramework.module respectively)

## Platform specific serialization
Serialization is a litte different for each platform because the strong names are different. Use the Platform dropdown list to select the desired serialization format. There are six app models listed but just three formats (for three platforms):

* .Net Framework stands alone
* UWP, Xamarin UWP and .Net Core (.Net Core platform)
* iOS and Android (Xamarin platform)
