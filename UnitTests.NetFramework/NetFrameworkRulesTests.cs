using LogicBuilder.Workflow.Activities.Rules;
using LogicBuilder.Workflow.ComponentModel.Compiler;
using LogicBuilder.Workflow.ComponentModel.Serialization;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;

namespace UnitTests.NetFramework
{
    public class NetFrameworkRulesTests
    {
        public NetFrameworkRulesTests()
        {
            CreateRuleEngine();
        }

        [Fact]
        public void Test_Rule_with_equals_condition_and_setter_action()
        {
            //Arrange
            SampleFlow.FlowEntity entity = new SampleFlow.FlowEntity
            {
                State = "CT"
            };

            //Act
            ruleEngine.Execute(entity);
            //Assert
            Assert.True(entity.Discount == 100);
        }

        [Fact]
        public void Test_Rule_with_array_indexer_condition_and_setter_action()
        {
            //Arrange
            SampleFlow.FlowEntity entity = new SampleFlow.FlowEntity();
            entity.StringList[1, 1] = "A";

            //Act
            ruleEngine.Execute(entity);
            //Assert
            Assert.True(entity.Discount == 200);
        }

        [Fact]
        public void Test_Rule_with_method_condition()
        {
            //Arrange
            SampleFlow.FlowEntity entity = new SampleFlow.FlowEntity
            {
                BoolText = "false"
            };

            //Act
            ruleEngine.Execute(entity);
            //Assert
            Assert.True(entity.Discount == 300);
        }

        [Fact]
        public void Test_Rule_with_multiple_conditions()
        {
            //Arrange
            SampleFlow.FlowEntity entity = new SampleFlow.FlowEntity
            {
                BoolText = "false",
                State = "MD"
            };

            //Act
            ruleEngine.Execute(entity);
            //Assert
            Assert.True(entity.Discount == 400);
        }

        [Fact]
        public void Test_Rule_with_static_method_action()
        {
            //Arrange
            SampleFlow.FlowEntity entity = new SampleFlow.FlowEntity
            {
                State = "MA"
            };

            //Act
            ruleEngine.Execute(entity);
            //Assert
            Assert.True(SampleFlow.FlowEntity.DEFAULTSTATE == "NH");
        }

        [Fact]
        public void Test_Rule_with_reference_method_action()
        {
            //Arrange
            SampleFlow.FlowEntity entity = new SampleFlow.FlowEntity
            {
                State = "PA"
            };

            //Act
            ruleEngine.Execute(entity);
            //Assert
            Assert.True(entity.TheType.FullName == typeof(SampleFlow.FlowEntity).FullName);
        }

        [Fact]
        public void Test_Rule_with_cast_object_expression_in_then_action()
        {
            //Arrange
            SampleFlow.FlowEntity entity = new SampleFlow.FlowEntity
            {
                State = "VA"
            };

            //Act
            ruleEngine.Execute(entity);
            //Assert
            Assert.True(((SampleFlow.ChildEntity)entity.DClass).Description == "This Description");
        }

        [Fact]
        public void Test_Rule_with_simple_CodeObjectCreateExpression_calling_a_constructor_in_then_action()
        {
            //Arrange
            SampleFlow.FlowEntity entity = new SampleFlow.FlowEntity
            {
                State = "NC"
            };

            //Act
            ruleEngine.Execute(entity);
            //Assert
            Assert.True(entity.FirstValue == "AAA");
        }

        [Fact]
        public void Test_Rule_with_list_initialization()
        {
            //Arrange
            SampleFlow.FlowEntity entity = new SampleFlow.FlowEntity
            {
                State = "SC"
            };

            //Act
            ruleEngine.Execute(entity);
            //Assert
            Assert.True(entity.MyCollection.First().ToString() == "AValue");
        }

        [Fact]
        public void Test_Rule_with_child_and_granchild_reference()
        {
            //Arrange
            SampleFlow.FlowEntity entity = new SampleFlow.FlowEntity
            {
                State = "GA"
            };

            //Act
            ruleEngine.Execute(entity);
            //Assert
            Assert.True(entity.FirstClass.SecondClass.Property1 == "This Value");
        }

        [Fact]
        public void Test_Rule_with_generic_object_initialization()
        {
            //Arrange
            SampleFlow.FlowEntity entity = new SampleFlow.FlowEntity
            {
                State = "TN"
            };

            //Act
            ruleEngine.Execute(entity);

            //Assert
            Assert.True(entity.GenericString.CurrentValue == "Stay");
        }

        [Fact]
        public void Test_Rule_with_generic_list_initialization()
        {
            //Arrange
            SampleFlow.FlowEntity entity = new SampleFlow.FlowEntity
            {
                State = "AL"
            };

            //Act
            ruleEngine.Execute(entity);

            //Assert
            Assert.True(entity.GenericListOfDecimal.CurrentValue[0] == 1.45m);
        }

        [Fact]
        public void TestSerialization()
        {
            //Arrange
            const string existing = "<RuleSet ChainingBehavior=\"Full\" Description=\"{p1:Null}\" Name=\"MyRuleSet\" xmlns:p1=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/workflow\">\r\n\t<RuleSet.Rules>\r\n\t\t<Rule Active=\"True\" Description=\"{p1:Null}\" Name=\"Rule0\" Priority=\"0\" ReevaluationBehavior=\"Always\">\r\n\t\t\t<Rule.Condition>\r\n\t\t\t\t<RuleExpressionCondition Name=\"{p1:Null}\">\r\n\t\t\t\t\t<RuleExpressionCondition.Expression>\r\n\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression Operator=\"ValueEquality\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"State\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">TX</ns1:String>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression>\r\n\t\t\t\t\t</RuleExpressionCondition.Expression>\r\n\t\t\t\t</RuleExpressionCondition>\r\n\t\t\t</Rule.Condition>\r\n\t\t\t<Rule.ThenActions>\r\n\t\t\t\t<RuleStatementAction>\r\n\t\t\t\t\t<RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t\t\t<ns0:CodeAssignStatement LinePragma=\"{p1:Null}\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"DClass2\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression MethodName=\"Parse\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeTypeReferenceExpression Type=\"System.DateTime, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\" />\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">01/03/2017</ns1:String>\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"InvariantCulture\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeTypeReferenceExpression Type=\"System.Globalization.CultureInfo, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\" />\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t</ns0:CodeAssignStatement>\r\n\t\t\t\t\t</RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t</RuleStatementAction>\r\n\t\t\t\t<RuleStatementAction>\r\n\t\t\t\t\t<RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t\t\t<ns0:CodeAssignStatement LinePragma=\"{p1:Null}\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"DClass2\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression MethodName=\"Parse\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeTypeReferenceExpression Type=\"System.TimeSpan, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\" />\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">12:3:5</ns1:String>\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"InvariantCulture\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeTypeReferenceExpression Type=\"System.Globalization.CultureInfo, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\" />\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t</ns0:CodeAssignStatement>\r\n\t\t\t\t\t</RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t</RuleStatementAction>\r\n\t\t\t\t<RuleStatementAction>\r\n\t\t\t\t\t<RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t\t\t<ns0:CodeAssignStatement LinePragma=\"{p1:Null}\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"DClass2\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression MethodName=\"Parse\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeTypeReferenceExpression Type=\"System.Guid, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\" />\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">{}{7E75868B-CDBE-408C-BEA2-88F887ACD725}</ns1:String>\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t</ns0:CodeAssignStatement>\r\n\t\t\t\t\t</RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t</RuleStatementAction>\r\n\t\t\t\t<RuleStatementAction>\r\n\t\t\t\t\t<RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t\t\t<ns0:CodeAssignStatement LinePragma=\"{p1:Null}\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"DClass2\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t<ns1:Decimal xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">100.0012</ns1:Decimal>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t</ns0:CodeAssignStatement>\r\n\t\t\t\t\t</RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t</RuleStatementAction>\r\n\t\t\t\t<RuleStatementAction>\r\n\t\t\t\t\t<RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t\t\t<ns0:CodeAssignStatement LinePragma=\"{p1:Null}\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"DClass2\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t<ns1:Decimal xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">100.0012</ns1:Decimal>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t</ns0:CodeAssignStatement>\r\n\t\t\t\t\t</RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t</RuleStatementAction>\r\n\t\t\t\t<RuleStatementAction>\r\n\t\t\t\t\t<RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t\t\t<ns0:CodeAssignStatement LinePragma=\"{p1:Null}\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"DClass2\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t<ns1:Char xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">P</ns1:Char>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t</ns0:CodeAssignStatement>\r\n\t\t\t\t\t</RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t</RuleStatementAction>\r\n\t\t\t\t<RuleStatementAction>\r\n\t\t\t\t\t<RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t\t\t<ns0:CodeAssignStatement LinePragma=\"{p1:Null}\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"DClass2\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t<ns1:Int32 xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">100</ns1:Int32>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t</ns0:CodeAssignStatement>\r\n\t\t\t\t\t</RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t</RuleStatementAction>\r\n\t\t\t\t<RuleStatementAction>\r\n\t\t\t\t\t<RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t\t\t<ns0:CodeAssignStatement LinePragma=\"{p1:Null}\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"DClass2\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t<ns1:Single xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">100.01</ns1:Single>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t</ns0:CodeAssignStatement>\r\n\t\t\t\t\t</RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t</RuleStatementAction>\r\n\t\t\t\t<RuleStatementAction>\r\n\t\t\t\t\t<RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t\t\t<ns0:CodeAssignStatement LinePragma=\"{p1:Null}\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"DClass2\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t<ns1:Single xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">100.0012</ns1:Single>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t</ns0:CodeAssignStatement>\r\n\t\t\t\t\t</RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t</RuleStatementAction>\r\n\t\t\t</Rule.ThenActions>\r\n\t\t</Rule>\r\n\t\t<Rule Active=\"True\" Description=\"{p1:Null}\" Name=\"Rule1\" Priority=\"0\" ReevaluationBehavior=\"Always\">\r\n\t\t\t<Rule.Condition>\r\n\t\t\t\t<RuleExpressionCondition Name=\"{p1:Null}\">\r\n\t\t\t\t\t<RuleExpressionCondition.Expression>\r\n\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression Operator=\"ValueEquality\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"State\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">CT</ns1:String>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression>\r\n\t\t\t\t\t</RuleExpressionCondition.Expression>\r\n\t\t\t\t</RuleExpressionCondition>\r\n\t\t\t</Rule.Condition>\r\n\t\t\t<Rule.ThenActions>\r\n\t\t\t\t<RuleStatementAction>\r\n\t\t\t\t\t<RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t\t\t<ns0:CodeAssignStatement LinePragma=\"{p1:Null}\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"Discount\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t<ns1:Int32 xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">100</ns1:Int32>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t</ns0:CodeAssignStatement>\r\n\t\t\t\t\t</RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t</RuleStatementAction>\r\n\t\t\t</Rule.ThenActions>\r\n\t\t</Rule>\r\n\t\t<Rule Active=\"True\" Description=\"{p1:Null}\" Name=\"Rule2\" Priority=\"0\" ReevaluationBehavior=\"Always\">\r\n\t\t\t<Rule.Condition>\r\n\t\t\t\t<RuleExpressionCondition Name=\"{p1:Null}\">\r\n\t\t\t\t\t<RuleExpressionCondition.Expression>\r\n\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression Operator=\"ValueEquality\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodeArrayIndexerExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeArrayIndexerExpression.Indices>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns1:Int32 xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">1</ns1:Int32>\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns1:Int32 xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">1</ns1:Int32>\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeArrayIndexerExpression.Indices>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeArrayIndexerExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"StringList\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeArrayIndexerExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodeArrayIndexerExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">A</ns1:String>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression>\r\n\t\t\t\t\t</RuleExpressionCondition.Expression>\r\n\t\t\t\t</RuleExpressionCondition>\r\n\t\t\t</Rule.Condition>\r\n\t\t\t<Rule.ThenActions>\r\n\t\t\t\t<RuleStatementAction>\r\n\t\t\t\t\t<RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t\t\t<ns0:CodeAssignStatement LinePragma=\"{p1:Null}\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"Discount\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t<ns1:Int32 xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">200</ns1:Int32>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t</ns0:CodeAssignStatement>\r\n\t\t\t\t\t</RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t</RuleStatementAction>\r\n\t\t\t</Rule.ThenActions>\r\n\t\t</Rule>\r\n\t\t<Rule Active=\"True\" Description=\"{p1:Null}\" Name=\"Rule3\" Priority=\"0\" ReevaluationBehavior=\"Always\">\r\n\t\t\t<Rule.Condition>\r\n\t\t\t\t<RuleExpressionCondition Name=\"{p1:Null}\">\r\n\t\t\t\t\t<RuleExpressionCondition.Expression>\r\n\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression MethodName=\"BoolMethod\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t</RuleExpressionCondition.Expression>\r\n\t\t\t\t</RuleExpressionCondition>\r\n\t\t\t</Rule.Condition>\r\n\t\t\t<Rule.ThenActions>\r\n\t\t\t\t<RuleStatementAction>\r\n\t\t\t\t\t<RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t\t\t<ns0:CodeAssignStatement LinePragma=\"{p1:Null}\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"Discount\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t<ns1:Int32 xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">300</ns1:Int32>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t</ns0:CodeAssignStatement>\r\n\t\t\t\t\t</RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t</RuleStatementAction>\r\n\t\t\t</Rule.ThenActions>\r\n\t\t</Rule>\r\n\t\t<Rule Active=\"True\" Description=\"{p1:Null}\" Name=\"Rule4\" Priority=\"0\" ReevaluationBehavior=\"Always\">\r\n\t\t\t<Rule.Condition>\r\n\t\t\t\t<RuleExpressionCondition Name=\"{p1:Null}\">\r\n\t\t\t\t\t<RuleExpressionCondition.Expression>\r\n\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression Operator=\"BooleanAnd\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression Operator=\"ValueEquality\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"State\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">MD</ns1:String>\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression MethodName=\"BoolMethod\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression>\r\n\t\t\t\t\t</RuleExpressionCondition.Expression>\r\n\t\t\t\t</RuleExpressionCondition>\r\n\t\t\t</Rule.Condition>\r\n\t\t\t<Rule.ThenActions>\r\n\t\t\t\t<RuleStatementAction>\r\n\t\t\t\t\t<RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t\t\t<ns0:CodeAssignStatement LinePragma=\"{p1:Null}\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"Discount\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t<ns1:Int32 xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">400</ns1:Int32>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t</ns0:CodeAssignStatement>\r\n\t\t\t\t\t</RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t</RuleStatementAction>\r\n\t\t\t</Rule.ThenActions>\r\n\t\t</Rule>\r\n\t\t<Rule Active=\"True\" Description=\"{p1:Null}\" Name=\"Rule5\" Priority=\"0\" ReevaluationBehavior=\"Always\">\r\n\t\t\t<Rule.Condition>\r\n\t\t\t\t<RuleExpressionCondition Name=\"{p1:Null}\">\r\n\t\t\t\t\t<RuleExpressionCondition.Expression>\r\n\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression Operator=\"ValueEquality\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"State\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">MA</ns1:String>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression>\r\n\t\t\t\t\t</RuleExpressionCondition.Expression>\r\n\t\t\t\t</RuleExpressionCondition>\r\n\t\t\t</Rule.Condition>\r\n\t\t\t<Rule.ThenActions>\r\n\t\t\t\t<RuleStatementAction>\r\n\t\t\t\t\t<RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t\t\t<ns0:CodeExpressionStatement LinePragma=\"{p1:Null}\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeExpressionStatement.Expression>\r\n\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression MethodName=\"SetDefaultState\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeTypeReferenceExpression Type=\"SampleFlow.FlowEntity\" />\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">NH</ns1:String>\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeExpressionStatement.Expression>\r\n\t\t\t\t\t\t</ns0:CodeExpressionStatement>\r\n\t\t\t\t\t</RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t</RuleStatementAction>\r\n\t\t\t</Rule.ThenActions>\r\n\t\t</Rule>\r\n\t\t<Rule Active=\"True\" Description=\"{p1:Null}\" Name=\"Rule6\" Priority=\"0\" ReevaluationBehavior=\"Always\">\r\n\t\t\t<Rule.Condition>\r\n\t\t\t\t<RuleExpressionCondition Name=\"{p1:Null}\">\r\n\t\t\t\t\t<RuleExpressionCondition.Expression>\r\n\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression Operator=\"ValueEquality\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"State\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">PA</ns1:String>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression>\r\n\t\t\t\t\t</RuleExpressionCondition.Expression>\r\n\t\t\t\t</RuleExpressionCondition>\r\n\t\t\t</Rule.Condition>\r\n\t\t\t<Rule.ThenActions>\r\n\t\t\t\t<RuleStatementAction>\r\n\t\t\t\t\t<RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t\t\t<ns0:CodeAssignStatement LinePragma=\"{p1:Null}\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"TheType\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression MethodName=\"GetType\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t</ns0:CodeAssignStatement>\r\n\t\t\t\t\t</RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t</RuleStatementAction>\r\n\t\t\t</Rule.ThenActions>\r\n\t\t</Rule>\r\n\t\t<Rule Active=\"True\" Description=\"{p1:Null}\" Name=\"Rule7\" Priority=\"0\" ReevaluationBehavior=\"Always\">\r\n\t\t\t<Rule.Condition>\r\n\t\t\t\t<RuleExpressionCondition Name=\"{p1:Null}\">\r\n\t\t\t\t\t<RuleExpressionCondition.Expression>\r\n\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression Operator=\"ValueEquality\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"State\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">VA</ns1:String>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression>\r\n\t\t\t\t\t</RuleExpressionCondition.Expression>\r\n\t\t\t\t</RuleExpressionCondition>\r\n\t\t\t</Rule.Condition>\r\n\t\t\t<Rule.ThenActions>\r\n\t\t\t\t<RuleStatementAction>\r\n\t\t\t\t\t<RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t\t\t<ns0:CodeAssignStatement LinePragma=\"{p1:Null}\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"Description\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeCastExpression TargetType=\"SampleFlow.ChildEntity\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeCastExpression.Expression>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeFieldReferenceExpression FieldName=\"DClass\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeFieldReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeFieldReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeFieldReferenceExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeCastExpression.Expression>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodeCastExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">This Description</ns1:String>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t</ns0:CodeAssignStatement>\r\n\t\t\t\t\t</RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t</RuleStatementAction>\r\n\t\t\t</Rule.ThenActions>\r\n\t\t</Rule>\r\n\t\t<Rule Active=\"True\" Description=\"{p1:Null}\" Name=\"Rule8\" Priority=\"0\" ReevaluationBehavior=\"Always\">\r\n\t\t\t<Rule.Condition>\r\n\t\t\t\t<RuleExpressionCondition Name=\"{p1:Null}\">\r\n\t\t\t\t\t<RuleExpressionCondition.Expression>\r\n\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression Operator=\"ValueEquality\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"State\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">NC</ns1:String>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression>\r\n\t\t\t\t\t</RuleExpressionCondition.Expression>\r\n\t\t\t\t</RuleExpressionCondition>\r\n\t\t\t</Rule.Condition>\r\n\t\t\t<Rule.ThenActions>\r\n\t\t\t\t<RuleStatementAction>\r\n\t\t\t\t\t<RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t\t\t<ns0:CodeExpressionStatement LinePragma=\"{p1:Null}\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeExpressionStatement.Expression>\r\n\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression MethodName=\"SetFirstValue\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeObjectCreateExpression CreateType=\"SampleFlow.OtherEntity\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeObjectCreateExpression.Parameters>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">AAA</ns1:String>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">BBB</ns1:String>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeObjectCreateExpression.Parameters>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodeObjectCreateExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeExpressionStatement.Expression>\r\n\t\t\t\t\t\t</ns0:CodeExpressionStatement>\r\n\t\t\t\t\t</RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t</RuleStatementAction>\r\n\t\t\t</Rule.ThenActions>\r\n\t\t</Rule>\r\n\t\t<Rule Active=\"True\" Description=\"{p1:Null}\" Name=\"Rule9\" Priority=\"0\" ReevaluationBehavior=\"Always\">\r\n\t\t\t<Rule.Condition>\r\n\t\t\t\t<RuleExpressionCondition Name=\"{p1:Null}\">\r\n\t\t\t\t\t<RuleExpressionCondition.Expression>\r\n\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression Operator=\"ValueEquality\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"State\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">SC</ns1:String>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression>\r\n\t\t\t\t\t</RuleExpressionCondition.Expression>\r\n\t\t\t\t</RuleExpressionCondition>\r\n\t\t\t</Rule.Condition>\r\n\t\t\t<Rule.ThenActions>\r\n\t\t\t\t<RuleStatementAction>\r\n\t\t\t\t\t<RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t\t\t<ns0:CodeExpressionStatement LinePragma=\"{p1:Null}\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeExpressionStatement.Expression>\r\n\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression MethodName=\"SetCollection\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeObjectCreateExpression CreateType=\"System.Collections.ObjectModel.Collection`1[[System.Object, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeObjectCreateExpression.Parameters>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeArrayCreateExpression CreateType=\"System.Object, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\" Size=\"0\" SizeExpression=\"{p1:Null}\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeArrayCreateExpression.Initializers>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">AValue</ns1:String>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">BValue</ns1:String>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeArrayCreateExpression.Initializers>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeArrayCreateExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeObjectCreateExpression.Parameters>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodeObjectCreateExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeExpressionStatement.Expression>\r\n\t\t\t\t\t\t</ns0:CodeExpressionStatement>\r\n\t\t\t\t\t</RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t</RuleStatementAction>\r\n\t\t\t</Rule.ThenActions>\r\n\t\t</Rule>\r\n\t\t<Rule Active=\"True\" Description=\"{p1:Null}\" Name=\"Rule10\" Priority=\"0\" ReevaluationBehavior=\"Always\">\r\n\t\t\t<Rule.Condition>\r\n\t\t\t\t<RuleExpressionCondition Name=\"{p1:Null}\">\r\n\t\t\t\t\t<RuleExpressionCondition.Expression>\r\n\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression Operator=\"ValueEquality\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"State\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">GA</ns1:String>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression>\r\n\t\t\t\t\t</RuleExpressionCondition.Expression>\r\n\t\t\t\t</RuleExpressionCondition>\r\n\t\t\t</Rule.Condition>\r\n\t\t\t<Rule.ThenActions>\r\n\t\t\t\t<RuleStatementAction>\r\n\t\t\t\t\t<RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t\t\t<ns0:CodeAssignStatement LinePragma=\"{p1:Null}\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"Property1\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"SecondClass\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"FirstClass\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">This Value</ns1:String>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t</ns0:CodeAssignStatement>\r\n\t\t\t\t\t</RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t</RuleStatementAction>\r\n\t\t\t</Rule.ThenActions>\r\n\t\t</Rule>\r\n\t\t<Rule Active=\"True\" Description=\"{p1:Null}\" Name=\"Rule11\" Priority=\"0\" ReevaluationBehavior=\"Always\">\r\n\t\t\t<Rule.Condition>\r\n\t\t\t\t<RuleExpressionCondition Name=\"{p1:Null}\">\r\n\t\t\t\t\t<RuleExpressionCondition.Expression>\r\n\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression Operator=\"ValueEquality\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"State\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">TN</ns1:String>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression>\r\n\t\t\t\t\t</RuleExpressionCondition.Expression>\r\n\t\t\t\t</RuleExpressionCondition>\r\n\t\t\t</Rule.Condition>\r\n\t\t\t<Rule.ThenActions>\r\n\t\t\t\t<RuleStatementAction>\r\n\t\t\t\t\t<RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t\t\t<ns0:CodeExpressionStatement LinePragma=\"{p1:Null}\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeExpressionStatement.Expression>\r\n\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression MethodName=\"SetGenericObject\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeObjectCreateExpression CreateType=\"SampleFlow.GenericClass`1[System.String]\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeObjectCreateExpression.Parameters>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<ns1:Int32 xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">7</ns1:Int32>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">VName</ns1:String>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">Stay</ns1:String>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">ObjectData</ns1:String>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeObjectCreateExpression.Parameters>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodeObjectCreateExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeExpressionStatement.Expression>\r\n\t\t\t\t\t\t</ns0:CodeExpressionStatement>\r\n\t\t\t\t\t</RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t</RuleStatementAction>\r\n\t\t\t</Rule.ThenActions>\r\n\t\t</Rule>\r\n\t\t<Rule Active=\"True\" Description=\"{p1:Null}\" Name=\"Rule12\" Priority=\"0\" ReevaluationBehavior=\"Always\">\r\n\t\t\t<Rule.Condition>\r\n\t\t\t\t<RuleExpressionCondition Name=\"{p1:Null}\">\r\n\t\t\t\t\t<RuleExpressionCondition.Expression>\r\n\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression Operator=\"ValueEquality\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"State\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">AL</ns1:String>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression>\r\n\t\t\t\t\t</RuleExpressionCondition.Expression>\r\n\t\t\t\t</RuleExpressionCondition>\r\n\t\t\t</Rule.Condition>\r\n\t\t\t<Rule.ThenActions>\r\n\t\t\t\t<RuleStatementAction>\r\n\t\t\t\t\t<RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t\t\t<ns0:CodeExpressionStatement LinePragma=\"{p1:Null}\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeExpressionStatement.Expression>\r\n\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression MethodName=\"SetGenericObject\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeObjectCreateExpression CreateType=\"SampleFlow.GenericClass`1[System.Collections.Generic.IList`1[System.Decimal]]\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeObjectCreateExpression.Parameters>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<ns1:Int32 xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">7</ns1:Int32>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">VName</ns1:String>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeObjectCreateExpression CreateType=\"System.Collections.Generic.List`1[[System.Decimal, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeObjectCreateExpression.Parameters>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeArrayCreateExpression CreateType=\"System.Decimal, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\" Size=\"0\" SizeExpression=\"{p1:Null}\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeArrayCreateExpression.Initializers>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<ns1:Decimal xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">1.45</ns1:Decimal>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<ns1:Decimal xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">2.35</ns1:Decimal>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeArrayCreateExpression.Initializers>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeArrayCreateExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeObjectCreateExpression.Parameters>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeObjectCreateExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">ObjectData</ns1:String>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeObjectCreateExpression.Parameters>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodeObjectCreateExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeExpressionStatement.Expression>\r\n\t\t\t\t\t\t</ns0:CodeExpressionStatement>\r\n\t\t\t\t\t</RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t</RuleStatementAction>\r\n\t\t\t</Rule.ThenActions>\r\n\t\t</Rule>\r\n\t\t<Rule Active=\"True\" Description=\"{p1:Null}\" Name=\"Rule13\" Priority=\"0\" ReevaluationBehavior=\"Always\">\r\n\t\t\t<Rule.Condition>\r\n\t\t\t\t<RuleExpressionCondition Name=\"{p1:Null}\">\r\n\t\t\t\t\t<RuleExpressionCondition.Expression>\r\n\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression Operator=\"ValueEquality\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"State\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">AZ</ns1:String>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression>\r\n\t\t\t\t\t</RuleExpressionCondition.Expression>\r\n\t\t\t\t</RuleExpressionCondition>\r\n\t\t\t</Rule.Condition>\r\n\t\t\t<Rule.ThenActions>\r\n\t\t\t\t<RuleStatementAction>\r\n\t\t\t\t\t<RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t\t\t<ns0:CodeAssignStatement LinePragma=\"{p1:Null}\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"MyArray\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression MethodName=\"StaticMethod\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeTypeReferenceExpression Type=\"SampleFlow.ListConverter`1[System.String]\" />\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"MyList\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeAssignStatement.Right>\r\n\t\t\t\t\t\t</ns0:CodeAssignStatement>\r\n\t\t\t\t\t</RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t</RuleStatementAction>\r\n\t\t\t</Rule.ThenActions>\r\n\t\t</Rule>\r\n\t\t<Rule Active=\"True\" Description=\"{p1:Null}\" Name=\"Rule14\" Priority=\"0\" ReevaluationBehavior=\"Always\">\r\n\t\t\t<Rule.Condition>\r\n\t\t\t\t<RuleExpressionCondition Name=\"{p1:Null}\">\r\n\t\t\t\t\t<RuleExpressionCondition.Expression>\r\n\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression Operator=\"ValueEquality\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression MethodName=\"GetDiscount\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeTypeReferenceExpression Type=\"SampleFlow.StaticClass\" />\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t<ns1:Decimal xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">101</ns1:Decimal>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression>\r\n\t\t\t\t\t</RuleExpressionCondition.Expression>\r\n\t\t\t\t</RuleExpressionCondition>\r\n\t\t\t</Rule.Condition>\r\n\t\t\t<Rule.ThenActions>\r\n\t\t\t\t<RuleStatementAction>\r\n\t\t\t\t\t<RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t\t\t<ns0:CodeExpressionStatement LinePragma=\"{p1:Null}\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeExpressionStatement.Expression>\r\n\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression MethodName=\"SetState\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeTypeReferenceExpression Type=\"SampleFlow.StaticClass\" />\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">OR</ns1:String>\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeExpressionStatement.Expression>\r\n\t\t\t\t\t\t</ns0:CodeExpressionStatement>\r\n\t\t\t\t\t</RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t</RuleStatementAction>\r\n\t\t\t</Rule.ThenActions>\r\n\t\t</Rule>\r\n\t\t<Rule Active=\"True\" Description=\"{p1:Null}\" Name=\"Rule15\" Priority=\"100\" ReevaluationBehavior=\"Always\">\r\n\t\t\t<Rule.Condition>\r\n\t\t\t\t<RuleExpressionCondition Name=\"{p1:Null}\">\r\n\t\t\t\t\t<RuleExpressionCondition.Expression>\r\n\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression Operator=\"ValueEquality\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression MethodName=\"GetState\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeTypeReferenceExpression Type=\"SampleFlow.StaticClass\" />\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">OR</ns1:String>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression>\r\n\t\t\t\t\t</RuleExpressionCondition.Expression>\r\n\t\t\t\t</RuleExpressionCondition>\r\n\t\t\t</Rule.Condition>\r\n\t\t\t<Rule.ThenActions>\r\n\t\t\t\t<RuleStatementAction>\r\n\t\t\t\t\t<RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t\t\t<ns0:CodeExpressionStatement LinePragma=\"{p1:Null}\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeExpressionStatement.Expression>\r\n\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression MethodName=\"SetDiscount\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeTypeReferenceExpression Type=\"SampleFlow.StaticClass\" />\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns1:Decimal xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">102</ns1:Decimal>\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeExpressionStatement.Expression>\r\n\t\t\t\t\t\t</ns0:CodeExpressionStatement>\r\n\t\t\t\t\t</RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t</RuleStatementAction>\r\n\t\t\t</Rule.ThenActions>\r\n\t\t</Rule>\r\n\t\t<Rule Active=\"True\" Description=\"{p1:Null}\" Name=\"Rule16\" Priority=\"0\" ReevaluationBehavior=\"Always\">\r\n\t\t\t<Rule.Condition>\r\n\t\t\t\t<RuleExpressionCondition Name=\"{p1:Null}\">\r\n\t\t\t\t\t<RuleExpressionCondition.Expression>\r\n\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression Operator=\"ValueEquality\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression MethodName=\"GetDiscount\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeTypeReferenceExpression Type=\"SampleFlow.StaticClass\" />\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t<ns1:Decimal xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">103</ns1:Decimal>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression>\r\n\t\t\t\t\t</RuleExpressionCondition.Expression>\r\n\t\t\t\t</RuleExpressionCondition>\r\n\t\t\t</Rule.Condition>\r\n\t\t\t<Rule.ThenActions>\r\n\t\t\t\t<RuleStatementAction>\r\n\t\t\t\t\t<RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t\t\t<ns0:CodeExpressionStatement LinePragma=\"{p1:Null}\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeExpressionStatement.Expression>\r\n\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression MethodName=\"SetState\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeTypeReferenceExpression Type=\"SampleFlow.StaticClass\" />\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">WA</ns1:String>\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeExpressionStatement.Expression>\r\n\t\t\t\t\t\t</ns0:CodeExpressionStatement>\r\n\t\t\t\t\t</RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t</RuleStatementAction>\r\n\t\t\t\t<RuleUpdateAction Path=\"this/AlwaysTrue\" />\r\n\t\t\t</Rule.ThenActions>\r\n\t\t</Rule>\r\n\t\t<Rule Active=\"True\" Description=\"{p1:Null}\" Name=\"Rule17\" Priority=\"100\" ReevaluationBehavior=\"Always\">\r\n\t\t\t<Rule.Condition>\r\n\t\t\t\t<RuleExpressionCondition Name=\"{p1:Null}\">\r\n\t\t\t\t\t<RuleExpressionCondition.Expression>\r\n\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression Operator=\"BooleanAnd\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression Operator=\"ValueEquality\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression MethodName=\"GetState\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeTypeReferenceExpression Type=\"SampleFlow.StaticClass\" />\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns1:String xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">WA</ns1:String>\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression Operator=\"ValueEquality\">\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression PropertyName=\"AlwaysTrue\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodePropertyReferenceExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Left>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns1:Boolean xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">true</ns1:Boolean>\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression.Right>\r\n\t\t\t\t\t\t</ns0:CodeBinaryOperatorExpression>\r\n\t\t\t\t\t</RuleExpressionCondition.Expression>\r\n\t\t\t\t</RuleExpressionCondition>\r\n\t\t\t</Rule.Condition>\r\n\t\t\t<Rule.ThenActions>\r\n\t\t\t\t<RuleStatementAction>\r\n\t\t\t\t\t<RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t\t\t<ns0:CodeExpressionStatement LinePragma=\"{p1:Null}\" xmlns:ns0=\"clr-namespace:System.CodeDom;Assembly=System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">\r\n\t\t\t\t\t\t\t<ns0:CodeExpressionStatement.Expression>\r\n\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression MethodName=\"SetDiscount\">\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns0:CodeTypeReferenceExpression Type=\"SampleFlow.StaticClass\" />\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression.TargetObject>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodeMethodReferenceExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Method>\r\n\t\t\t\t\t\t\t\t\t<ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodeThisReferenceExpression />\r\n\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t\t\t<ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<ns1:Decimal xmlns:ns1=\"clr-namespace:System;Assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">104</ns1:Decimal>\r\n\t\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression.Value>\r\n\t\t\t\t\t\t\t\t\t\t</ns0:CodePrimitiveExpression>\r\n\t\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression.Parameters>\r\n\t\t\t\t\t\t\t\t</ns0:CodeMethodInvokeExpression>\r\n\t\t\t\t\t\t\t</ns0:CodeExpressionStatement.Expression>\r\n\t\t\t\t\t\t</ns0:CodeExpressionStatement>\r\n\t\t\t\t\t</RuleStatementAction.CodeDomStatement>\r\n\t\t\t\t</RuleStatementAction>\r\n\t\t\t</Rule.ThenActions>\r\n\t\t</Rule>\r\n\t</RuleSet.Rules>\r\n</RuleSet>";
            //Act
            string rulesSetString = SerializeRules(ruleSet);
            //Assert
            //Assert.Equal(rulesSetString, existing);
        }

        [Fact]
        public void Test_Rule_set_literals()
        {
            //Arrange
            SampleFlow.FlowEntity entity = new SampleFlow.FlowEntity
            {
                State = "TX"
            };

            //Act
            ruleEngine.Execute(entity);

            //Assert
            Assert.True((float)entity.DClass2 == 100.0012f);
        }

        [Fact]
        public void Test_Rule_call_method_in_static_generic_class()
        {
            //Arrange
            SampleFlow.FlowEntity entity = new SampleFlow.FlowEntity
            {
                State = "AZ"
            };

            //Act
            ruleEngine.Execute(entity);

            //Assert
            Assert.True(entity.MyArray[0] == "Apple");
        }

        [Fact]
        public void Test_Rule_reevaluation_WITHOUT_update_action_and_alwaysTrue_property()
        {
            //Arrange
            SampleFlow.FlowEntity entity = new SampleFlow.FlowEntity
            {
                Discount = 101m
            };

            //Act
            ruleEngine.Execute(entity);

            //Assert
            Assert.True(entity.Discount == 101m);
            Assert.True(entity.State == "OR");
        }

        [Fact]
        public void Test_Rule_reevaluation_WITH_update_action_and_alwaysTrue_property()
        {
            //Arrange
            SampleFlow.FlowEntity entity = new SampleFlow.FlowEntity
            {
                Discount = 103m
            };

            //Act
            ruleEngine.Execute(entity);

            //Assert
            Assert.True(entity.Discount == 104m);
            Assert.True(entity.State == "WA");
        }

        private static Rule Rule_set_literals()
        {
            CodePropertyReferenceExpression invariantCultureReference = new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(System.Globalization.CultureInfo)), "InvariantCulture");

            CodeBinaryOperatorExpression ruleStateTest = new CodeBinaryOperatorExpression
            {
                Left = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "State"),
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = new CodePrimitiveExpression("TX")
            };

            CodePropertyReferenceExpression dClassRef = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "DClass2");

            CodeAssignStatement dateAction = new CodeAssignStatement
                (
                    dClassRef,
                    new CodeMethodInvokeExpression
                        (
                            new CodeTypeReferenceExpression(typeof(System.DateTime)),
                            "Parse",
                            new CodeExpression[] { new CodePrimitiveExpression("01/03/2017"), invariantCultureReference }
                        )
                );

            CodeAssignStatement timeSpanAction = new CodeAssignStatement
                (
                    dClassRef,
                    new CodeMethodInvokeExpression
                        (
                            new CodeTypeReferenceExpression(typeof(System.TimeSpan)),
                            "Parse",
                            new CodeExpression[] { new CodePrimitiveExpression("12:3:5"), invariantCultureReference }
                        )
                );
            //System.Guid.Parse()
            CodeAssignStatement guidAction = new CodeAssignStatement
                (
                    dClassRef,
                    new CodeMethodInvokeExpression
                        (
                            new CodeTypeReferenceExpression(typeof(System.Guid)),
                            "Parse",
                            new CodeExpression[] { new CodePrimitiveExpression("{7E75868B-CDBE-408C-BEA2-88F887ACD725}") }
                        )
                );
            //decimal d = decimal.Parse("100.0012M", System.Globalization.CultureInfo.InvariantCulture);
            CodeAssignStatement decimalAction = new CodeAssignStatement(dClassRef, new CodePrimitiveExpression(100.0012m));
            CodeAssignStatement decimalAction1 = new CodeAssignStatement(dClassRef, new CodePrimitiveExpression(decimal.Parse("100.0012", System.Globalization.CultureInfo.InvariantCulture)));
            CodeAssignStatement charAction = new CodeAssignStatement(dClassRef, new CodePrimitiveExpression(char.Parse("P")));
            CodeAssignStatement intAction = new CodeAssignStatement(dClassRef, new CodePrimitiveExpression(100));
            CodeAssignStatement floatAction = new CodeAssignStatement(dClassRef, new CodePrimitiveExpression(100.01f));
            CodeAssignStatement floatAction2 = new CodeAssignStatement(dClassRef, new CodePrimitiveExpression(float.Parse("100.0012")));

            Rule rule0 = new Rule("Rule0")
            {
                Condition = new RuleExpressionCondition(ruleStateTest)
            };

            rule0.ThenActions.Add(new RuleStatementAction(dateAction));
            rule0.ThenActions.Add(new RuleStatementAction(timeSpanAction));
            rule0.ThenActions.Add(new RuleStatementAction(guidAction));
            rule0.ThenActions.Add(new RuleStatementAction(decimalAction));
            rule0.ThenActions.Add(new RuleStatementAction(decimalAction1));
            rule0.ThenActions.Add(new RuleStatementAction(charAction));
            rule0.ThenActions.Add(new RuleStatementAction(intAction));
            rule0.ThenActions.Add(new RuleStatementAction(floatAction));
            rule0.ThenActions.Add(new RuleStatementAction(floatAction2));

            return rule0;
        }

        private Rule Rule_with_equals_condition_and_setter_action()
        {
            // define first predicate: this.State == "CT"
            CodeBinaryOperatorExpression ruleStateTest = new CodeBinaryOperatorExpression
            {
                Left = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "State"),
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = new CodePrimitiveExpression("CT")
            };

            //discount action this.Discount = 100
            CodeAssignStatement discountAction = new CodeAssignStatement
                (
                    new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Discount"),
                    new CodePrimitiveExpression(100)
                );

            Rule rule1 = new Rule("Rule1")
            {
                Condition = new RuleExpressionCondition(ruleStateTest)
            };
            rule1.ThenActions.Add(new RuleStatementAction(discountAction));

            return rule1;
        }

        private Rule Rule_with_array_indexer_condition_and_setter_action()
        {
            //this.StringList[1, 1]
            CodeArrayIndexerExpression indexerExpression = new CodeArrayIndexerExpression
                (
                    new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "StringList"),
                    new CodeExpression[] { new CodePrimitiveExpression(1), new CodePrimitiveExpression(1) }
                );

            //this.StringList[1, 1] == "A"
            CodeBinaryOperatorExpression stringIndexTest = new CodeBinaryOperatorExpression
            {
                Left = indexerExpression,
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = new CodePrimitiveExpression("A")
            };

            //discount action this.Discount = 200
            CodeAssignStatement discountAction = new CodeAssignStatement
                (
                    new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Discount"),
                    new CodePrimitiveExpression(200)
                );

            Rule rule2 = new Rule("Rule2")
            {
                Condition = new RuleExpressionCondition(stringIndexTest)
            };
            rule2.ThenActions.Add(new RuleStatementAction(discountAction));

            return rule2;
        }


        private Rule Rule_with_method_condition()
        {
            //this.boolMethod()
            CodeMethodInvokeExpression boolMethodInvoke = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "BoolMethod", new CodeExpression[] { });

            //discount action this.discount = 300
            CodeAssignStatement discountAction = new CodeAssignStatement
                (
                    new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Discount"),
                    new CodePrimitiveExpression(300)
                );

            Rule rule3 = new Rule("Rule3")
            {
                Condition = new RuleExpressionCondition(boolMethodInvoke)
            };
            rule3.ThenActions.Add(new RuleStatementAction(discountAction));

            return rule3;
        }


        private Rule Rule_with_multiple_conditions()
        {
            // define first predicate: this.State == "MD"
            CodeBinaryOperatorExpression ruleStateTest = new CodeBinaryOperatorExpression
            {
                Left = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "State"),
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = new CodePrimitiveExpression("MD")
            };

            //this.boolMethod()
            CodeMethodInvokeExpression boolMethodInvoke = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "BoolMethod", new CodeExpression[] { });

            //combine both expressions this.state == "MD" && this.boolMethod()
            CodeBinaryOperatorExpression codeBothExpression = new CodeBinaryOperatorExpression
            {
                Left = ruleStateTest,
                Operator = CodeBinaryOperatorType.BooleanAnd,
                Right = boolMethodInvoke
            };

            //discount action this.Discount = 400
            CodeAssignStatement discountAction = new CodeAssignStatement
                (
                    new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Discount"),
                    new CodePrimitiveExpression(400)
                );

            Rule rule4 = new Rule("Rule4")
            {
                Condition = new RuleExpressionCondition(codeBothExpression)
            };
            rule4.ThenActions.Add(new RuleStatementAction(discountAction));

            return rule4;
        }


        private Rule Rule_with_static_method_action()
        {
            // define first predicate: this.State == "MA"
            CodeBinaryOperatorExpression ruleStateTest = new CodeBinaryOperatorExpression
            {
                Left = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "State"),
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = new CodePrimitiveExpression("MA")
            };

            //action SampleFlow.FlowEntity.SetDefaultState("NH")
            CodeMethodInvokeExpression methodInvoke = new CodeMethodInvokeExpression
                (
                    new CodeTypeReferenceExpression("SampleFlow.FlowEntity"),
                    "SetDefaultState",
                    new CodeExpression[] { new CodePrimitiveExpression("NH") }
                );

            Rule rule5 = new Rule("Rule5")
            {
                Condition = new RuleExpressionCondition(ruleStateTest)
            };
            rule5.ThenActions.Add(new RuleStatementAction(methodInvoke));

            return rule5;
        }


        private Rule Rule_with_reference_method_action()
        {
            // define first predicate: this.State == "PA"
            CodeBinaryOperatorExpression ruleStateTest = new CodeBinaryOperatorExpression
            {
                Left = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "State"),
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = new CodePrimitiveExpression("PA")
            };


            //action this.TheType = this.GetType
            CodeAssignStatement setTypeAction = new CodeAssignStatement
                (
                    new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "TheType"),
                    new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "GetType", new CodeExpression[] { })
                );

            Rule rule6 = new Rule("Rule6")
            {
                Condition = new RuleExpressionCondition(ruleStateTest)
            };
            rule6.ThenActions.Add(new RuleStatementAction(setTypeAction));

            return rule6;
        }


        private Rule Rule_with_Cast_object_expression_in_then_action()
        {
            // define first predicate: this.State == "VA"
            CodeBinaryOperatorExpression ruleStateTest = new CodeBinaryOperatorExpression
            {
                Left = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "State"),
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = new CodePrimitiveExpression("VA")
            };

            //(SampleFlow.ChildEntity)this.DClass
            CodeCastExpression castExpression = new CodeCastExpression
                (
                "SampleFlow.ChildEntity",
                 //this.DClass
                 new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "DClass")
                 );

            //((SampleFlow.ChildEntity)this.DClass).Description = "This Description"
            CodeAssignStatement assignmentAction = new CodeAssignStatement
                (
                    new CodePropertyReferenceExpression(castExpression, "Description"),
                    new CodePrimitiveExpression("This Description")
                );

            Rule rule7 = new Rule("Rule7")
            {
                Condition = new RuleExpressionCondition(ruleStateTest)
            };
            rule7.ThenActions.Add(new RuleStatementAction(assignmentAction));

            return rule7;
        }


        private Rule Rule_with_simple_CodeObjectCreateExpression_calling_a_constructor_in_then_action()
        {
            // define first predicate: this.State == "NC"
            CodeBinaryOperatorExpression ruleStateTest = new CodeBinaryOperatorExpression
            {
                Left = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "State"),
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = new CodePrimitiveExpression("NC")
            };

            //this.DoNothing(new SampleFlow.OtherEntity("AAA", "BBB"))
            CodeMethodInvokeExpression methodInvokeDoNothing = new CodeMethodInvokeExpression
                (
                    new CodeThisReferenceExpression(),
                    "SetFirstValue",
                    new CodeObjectCreateExpression
                        (
                            new CodeTypeReference("SampleFlow.OtherEntity"),
                            new CodeExpression[]
                            {
                                new CodePrimitiveExpression("AAA"),
                                new CodePrimitiveExpression("BBB")
                            }
                        )
                );

            Rule rule8 = new Rule("Rule8")
            {
                Condition = new RuleExpressionCondition(ruleStateTest)
            };
            rule8.ThenActions.Add(new RuleStatementAction(methodInvokeDoNothing));

            return rule8;
        }

        private Rule Rule_with_list_initialization()
        {
            // define first predicate: this.State == "SC"
            CodeBinaryOperatorExpression ruleStateTest = new CodeBinaryOperatorExpression
            {
                Left = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "State"),
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = new CodePrimitiveExpression("SC")
            };

            //this.SetCollection(new System.Collections.ObjectModel.Collection<object>(new object[] { "AValue", "BValue"}))
            CodeMethodInvokeExpression methodInvokeSetCollection = new CodeMethodInvokeExpression
                (
                    new CodeThisReferenceExpression(),
                    "SetCollection",
                    new CodeObjectCreateExpression
                        (
                            new CodeTypeReference
                                (
                                    "System.Collections.ObjectModel.Collection",
                                    new CodeTypeReference[] { new CodeTypeReference("System.Object") }
                                ),
                            new CodeArrayCreateExpression
                            (
                                "System.Object",
                                new CodeExpression[] { new CodePrimitiveExpression("AValue"), new CodePrimitiveExpression("BValue") }
                            )
                        )
                );

            Rule rule9 = new Rule("Rule9")
            {
                Condition = new RuleExpressionCondition(ruleStateTest)
            };
            rule9.ThenActions.Add(new RuleStatementAction(methodInvokeSetCollection));

            return rule9;
        }

        private Rule Rule_with_child_and_granchild_reference()
        {
            // define first predicate: this.State == "GA"
            CodeBinaryOperatorExpression ruleStateTest = new CodeBinaryOperatorExpression
            {
                Left = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "State"),
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = new CodePrimitiveExpression("GA")
            };

            //this.FirstClass.SecondClass.Property1 = "This Value"
            CodeAssignStatement setProperty1Action = new CodeAssignStatement
                (
                    new CodePropertyReferenceExpression
                    (
                        new CodePropertyReferenceExpression
                        (
                            new CodePropertyReferenceExpression
                            (
                                new CodeThisReferenceExpression(),
                                "FirstClass"
                            ),
                            "SecondClass"
                        ),
                        "Property1"
                    ),
                    new CodePrimitiveExpression("This Value")
                );

            Rule rule10 = new Rule("Rule10")
            {
                Condition = new RuleExpressionCondition(ruleStateTest)
            };
            rule10.ThenActions.Add(new RuleStatementAction(setProperty1Action));

            return rule10;
        }

        private static Rule Rule_with_generic_object_initialization()
        {
            // define first predicate: this.State == "TN"
            CodeBinaryOperatorExpression ruleStateTest = new CodeBinaryOperatorExpression
            {
                Left = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "State"),
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = new CodePrimitiveExpression("TN")
            };

            //this.SetGenericObject(new SampleFlow.GenericClass<string>(7, "VName", "Stay", "ObjectData")
            CodeMethodInvokeExpression methodInvokeSetGenericObject = new CodeMethodInvokeExpression
                (
                    new CodeThisReferenceExpression(),
                    "SetGenericObject",
                    new CodeObjectCreateExpression
                        (
                            new CodeTypeReference
                                (
                                    "SampleFlow.GenericClass",
                                    new CodeTypeReference[] { new CodeTypeReference("System.String") }
                                ),
                            new CodePrimitiveExpression(7),
                            new CodePrimitiveExpression("VName"),
                            new CodePrimitiveExpression("Stay"),
                            new CodePrimitiveExpression("ObjectData")
                        )
                );

            Rule rule11 = new Rule("Rule11")
            {
                Condition = new RuleExpressionCondition(ruleStateTest)
            };
            rule11.ThenActions.Add(new RuleStatementAction(methodInvokeSetGenericObject));

            return rule11;
        }

        private static Rule Rule_with_generic_list_initialization()
        {
            // define first predicate: this.State == "AL"
            CodeBinaryOperatorExpression ruleStateTest = new CodeBinaryOperatorExpression
            {
                Left = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "State"),
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = new CodePrimitiveExpression("AL")
            };

            //this.SetGenericObject(new SampleFlow.GenericClass<string>(7, "VName", new List<decimal> { 1.45m, 2.35m }, "ObjectData")
            CodeMethodInvokeExpression methodInvokeSetGenericList = new CodeMethodInvokeExpression
                (
                    new CodeThisReferenceExpression(),
                    "SetGenericObject",
                    new CodeObjectCreateExpression
                        (
                            new CodeTypeReference
                                (
                                    "SampleFlow.GenericClass",
                                    new CodeTypeReference[] { new CodeTypeReference("System.Collections.Generic.IList`1[[System.Decimal]]") }
                                ),
                            new CodePrimitiveExpression(7),
                            new CodePrimitiveExpression("VName"),
                            new CodeObjectCreateExpression
                            (
                                new CodeTypeReference
                                    (
                                        "System.Collections.Generic.List",
                                        new CodeTypeReference[] { new CodeTypeReference("System.Decimal") }
                                    ),
                                new CodeArrayCreateExpression
                                (
                                    "System.Decimal",
                                    new CodeExpression[] { new CodePrimitiveExpression(1.45m), new CodePrimitiveExpression(2.35m) }
                                )
                            ),
                            new CodePrimitiveExpression("ObjectData")
                        )
                );

            Rule rule12 = new Rule("Rule12")
            {
                Condition = new RuleExpressionCondition(ruleStateTest)
            };
            rule12.ThenActions.Add(new RuleStatementAction(methodInvokeSetGenericList));

            return rule12;
        }

        private static Rule Rule_call_method_in_static_generic_class()
        {
            CodeBinaryOperatorExpression ruleStateTest = new CodeBinaryOperatorExpression
            {
                Left = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "State"),
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = new CodePrimitiveExpression("AZ")
            };

            CodePropertyReferenceExpression myArrayRef = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "MyArray");
            CodePropertyReferenceExpression myListRef = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "MyList");

            CodeAssignStatement convertListToArray = new CodeAssignStatement
                (
                    myArrayRef,
                    new CodeMethodInvokeExpression
                        (
                            new CodeTypeReferenceExpression
                                (
                                    new CodeTypeReference("SampleFlow.ListConverter",
                                    new CodeTypeReference[] { new CodeTypeReference("System.String") })
                                ),
                            "StaticMethod",
                            myListRef
                        )
                );

            Rule rule13 = new Rule("Rule13")
            {
                Condition = new RuleExpressionCondition(ruleStateTest)
            };

            rule13.ThenActions.Add(new RuleStatementAction(convertListToArray));

            return rule13;
        }

        private static Rule Rule_set_state_WITHOUT_update_action()
        {
            CodeBinaryOperatorExpression ruleDiscountTest = new CodeBinaryOperatorExpression
            {
                Left = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("SampleFlow.StaticClass"), "GetDiscount", new CodeExpression[] { new CodeThisReferenceExpression() }),
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = new CodePrimitiveExpression(101m)
            };

            CodeMethodInvokeExpression setState = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("SampleFlow.StaticClass"), "SetState", new CodeExpression[] { new CodeThisReferenceExpression(), new CodePrimitiveExpression("OR") });

            Rule rule14 = new Rule("Rule14")
            {
                Condition = new RuleExpressionCondition(ruleDiscountTest)
            };

            rule14.ThenActions.Add(new RuleStatementAction(setState));

            return rule14;
        }

        private static Rule Rule_get_state_WITHOUT_AlwaysTrue_property_for_reevaluation()
        {
            CodeBinaryOperatorExpression ruleStateTest = new CodeBinaryOperatorExpression
            {
                Left = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("SampleFlow.StaticClass"), "GetState", new CodeExpression[] { new CodeThisReferenceExpression() }),
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = new CodePrimitiveExpression("OR")
            };

            CodeMethodInvokeExpression setDiscount = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("SampleFlow.StaticClass"), "SetDiscount", new CodeExpression[] { new CodeThisReferenceExpression(), new CodePrimitiveExpression(102m) });

            Rule rule15 = new Rule("Rule15")
            {
                Condition = new RuleExpressionCondition(ruleStateTest),
                Priority = 100
            };

            rule15.ThenActions.Add(new RuleStatementAction(setDiscount));

            return rule15;
        }

        private static Rule Rule_set_state_WITH_update_action_set_targeting_AlwaysTrue_property()
        {
            CodeBinaryOperatorExpression ruleDiscountTest = new CodeBinaryOperatorExpression
            {
                Left = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("SampleFlow.StaticClass"), "GetDiscount", new CodeExpression[] { new CodeThisReferenceExpression() }),
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = new CodePrimitiveExpression(103m)
            };

            CodeMethodInvokeExpression setState = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("SampleFlow.StaticClass"), "SetState", new CodeExpression[] { new CodeThisReferenceExpression(), new CodePrimitiveExpression("WA") });

            Rule rule16 = new Rule("Rule16")
            {
                Condition = new RuleExpressionCondition(ruleDiscountTest),
                Priority = 0
            };

            rule16.ThenActions.Add(new RuleStatementAction(setState));
            rule16.ThenActions.Add(new RuleUpdateAction("this/AlwaysTrue"));

            return rule16;
        }

        private static Rule Rule_get_state_WITH_AlwaysTrue_property_to_reevaluate_update_action()
        {
            CodeBinaryOperatorExpression ruleStateTest = new CodeBinaryOperatorExpression
            {
                Left = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("SampleFlow.StaticClass"), "GetState", new CodeExpression[] { new CodeThisReferenceExpression() }),
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = new CodePrimitiveExpression("WA")
            };

            CodeBinaryOperatorExpression ruleAlwaysTrueTest = new CodeBinaryOperatorExpression
            {
                Left = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "AlwaysTrue"),
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = new CodePrimitiveExpression(true)
            };

            CodeBinaryOperatorExpression ruleBothTest = new CodeBinaryOperatorExpression
            {
                Left = ruleStateTest,
                Operator = CodeBinaryOperatorType.BooleanAnd,
                Right = ruleAlwaysTrueTest
            };

            CodeMethodInvokeExpression setDiscount = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("SampleFlow.StaticClass"), "SetDiscount", new CodeExpression[] { new CodeThisReferenceExpression(), new CodePrimitiveExpression(104m) });

            Rule rule17 = new Rule("Rule17")
            {
                Condition = new RuleExpressionCondition(ruleBothTest),
                Priority = 100
            };

            rule17.ThenActions.Add(new RuleStatementAction(setDiscount));

            return rule17;
        }

        private void CreateRuleEngine()
        {
            ruleSet = new RuleSet
            {
                Name = "MyRuleSet",
                ChainingBehavior = RuleChainingBehavior.Full
            };

            ruleSet.Rules.Add(Rule_set_literals());
            ruleSet.Rules.Add(Rule_with_equals_condition_and_setter_action());
            ruleSet.Rules.Add(Rule_with_array_indexer_condition_and_setter_action());
            ruleSet.Rules.Add(Rule_with_method_condition());
            ruleSet.Rules.Add(Rule_with_multiple_conditions());
            ruleSet.Rules.Add(Rule_with_static_method_action());
            ruleSet.Rules.Add(Rule_with_reference_method_action());
            ruleSet.Rules.Add(Rule_with_Cast_object_expression_in_then_action());
            ruleSet.Rules.Add(Rule_with_simple_CodeObjectCreateExpression_calling_a_constructor_in_then_action());
            ruleSet.Rules.Add(Rule_with_list_initialization());
            ruleSet.Rules.Add(Rule_with_child_and_granchild_reference());
            ruleSet.Rules.Add(Rule_with_generic_object_initialization());
            ruleSet.Rules.Add(Rule_with_generic_list_initialization());
            ruleSet.Rules.Add(Rule_call_method_in_static_generic_class());
            ruleSet.Rules.Add(Rule_set_state_WITHOUT_update_action());
            ruleSet.Rules.Add(Rule_get_state_WITHOUT_AlwaysTrue_property_for_reevaluation());
            ruleSet.Rules.Add(Rule_set_state_WITH_update_action_set_targeting_AlwaysTrue_property());
            ruleSet.Rules.Add(Rule_get_state_WITH_AlwaysTrue_property_to_reevaluate_update_action());

            string ruleSetString = SerializeRules(ruleSet);

            ruleSet = DeserializeRuleSet(ruleSetString);

            RuleValidation ruleValidation = GetValidation(ruleSet, typeof(SampleFlow.FlowEntity));
            ruleEngine = new RuleEngine(ruleSet, ruleValidation);
        }

        private RuleSet ruleSet;
        private RuleEngine ruleEngine;

        private RuleValidation GetValidation(RuleSet ruleSet, Type type)
        {
            RuleValidation ruleValidation = null;

            if (ruleSet == null)
                throw new InvalidOperationException(Resources.ruleSetCannotBeNull);

            ruleValidation = new RuleValidation(type);
            if (ruleValidation == null)
                throw new InvalidOperationException(Resources.ruleValidationCannotBeNull);

            if (!ruleSet.Validate(ruleValidation))
            {
                List<string> errors = ruleValidation.Errors.Aggregate(new List<string> { string.Format(CultureInfo.CurrentCulture, Resources.invalidRuleSetFormat, ruleSet.Name) }, (list, next) =>
                {
                    list.Add(next.ErrorText);
                    return list;
                });

                throw new InvalidOperationException(string.Join(Environment.NewLine, errors));

            }

            return ruleValidation;
        }

        private string SerializeRules(object drs)
        {
            System.Text.StringBuilder ruleDefinition = new System.Text.StringBuilder();
            WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();
            using (System.IO.StringWriter stringWriter = new System.IO.StringWriter(ruleDefinition, System.Globalization.CultureInfo.InvariantCulture))
            {
                using (System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(stringWriter))
                {
                    serializer.Serialize(writer, drs);
                    writer.Flush();
                }
                stringWriter.Flush();
            }

            return ruleDefinition.ToString();
        }

        private RuleSet DeserializeRuleSet(string ruleSetXmlDefinition)
        {

            WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();
            if (!string.IsNullOrEmpty(ruleSetXmlDefinition))
            {
                using (System.IO.StringReader stringReader = new System.IO.StringReader(ruleSetXmlDefinition))
                {
                    using (System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader(stringReader))
                    {
                        return serializer.Deserialize(reader) as RuleSet;
                    }
                }
            }
            else
            {
                return null;
            }
        }
    }
}
