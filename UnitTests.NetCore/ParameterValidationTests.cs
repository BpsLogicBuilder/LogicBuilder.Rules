using LogicBuilder.Workflow.Activities.Rules;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;

namespace UnitTests.NetCore
{
    public class ParameterValidationTests
    {
        [Fact]
        public void Test_Rule_method_invocation_missing_required_parameter_throws_InvalidOperationException()
        {
            //Arrange
            RuleSet ruleSet = new RuleSet { Name = "MyRuleSet", ChainingBehavior = RuleChainingBehavior.Full };
            ruleSet.Rules.Add(BuildRule());

            //Act
            Assert.Throws<InvalidOperationException>(() => CreateRuleEngine(ruleSet));

            Rule BuildRule()
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
                    new CodeThisReferenceExpression(),
                    "SetValues",
                    new CodeExpression[] { new CodePrimitiveExpression("AAA") }
                );

                return new Rule("Rule1")
                {
                    Condition = new RuleExpressionCondition(ruleStateTest),
                    ThenActions = { new RuleStatementAction(methodInvoke) }
                };
            }
        }

        [Fact]
        public void Test_Rule_constructor_invocation_missing_required_parameter_throws_InvalidOperationException()
        {
            //Arrange
            RuleSet ruleSet = new RuleSet { Name = "MyRuleSet", ChainingBehavior = RuleChainingBehavior.Full };
            ruleSet.Rules.Add(BuildRule());

            //Act
            Assert.Throws<InvalidOperationException>(() => CreateRuleEngine(ruleSet));

            Rule BuildRule()
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
                    new CodeThisReferenceExpression(),
                    "SetValues",
                    new CodeObjectCreateExpression
                    (
                        new CodeTypeReference("SampleFlow.OtherEntity"),
                        new CodeExpression[]
                        {
                            new CodePrimitiveExpression("AAA")
                        }
                    )
                );

                return new Rule("Rule1")
                {
                    Condition = new RuleExpressionCondition(ruleStateTest),
                    ThenActions = { new RuleStatementAction(methodInvoke) }
                };
            }
        }

        [Fact]
        public void Test_Rule_method_invocation_with_parameter_type_mismatch_throws_InvalidOperationException()
        {
            //Arrange
            RuleSet ruleSet = new RuleSet { Name = "MyRuleSet", ChainingBehavior = RuleChainingBehavior.Full };
            ruleSet.Rules.Add(BuildRule());

            //Act
            Assert.Throws<InvalidOperationException>(() => CreateRuleEngine(ruleSet));

            Rule BuildRule()
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
                    new CodeThisReferenceExpression(),
                    "SetValues",
                    new CodeExpression[]
                    {
                        new CodePrimitiveExpression(1),
                        new CodePrimitiveExpression("BBB")
                    }
                );

                return new Rule("Rule1")
                {
                    Condition = new RuleExpressionCondition(ruleStateTest),
                    ThenActions = { new RuleStatementAction(methodInvoke) }
                };
            }
        }

        [Fact]
        public void Test_Rule_constructor_invocation_with_parameter_type_mismatch_throws_InvalidOperationException()
        {
            //Arrange
            RuleSet ruleSet = new RuleSet { Name = "MyRuleSet", ChainingBehavior = RuleChainingBehavior.Full };
            ruleSet.Rules.Add(BuildRule());

            //Act
            Assert.Throws<InvalidOperationException>(() => CreateRuleEngine(ruleSet));

            Rule BuildRule()
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
                    new CodeThisReferenceExpression(),
                    "SetValues",
                    new CodeObjectCreateExpression
                    (
                        new CodeTypeReference("SampleFlow.OtherEntity"),
                        new CodeExpression[]
                        {
                            new CodePrimitiveExpression(1),
                            new CodePrimitiveExpression("BBB")
                        }
                    )
                );

                return new Rule("Rule1")
                {
                    Condition = new RuleExpressionCondition(ruleStateTest),
                    ThenActions = { new RuleStatementAction(methodInvoke) }
                };
            }
        }

        [Fact]
        public void Test_Rule_method_invocation_with_too_many_parameters_throws_InvalidOperationException()
        {
            //Arrange
            RuleSet ruleSet = new RuleSet { Name = "MyRuleSet", ChainingBehavior = RuleChainingBehavior.Full };
            ruleSet.Rules.Add(BuildRule());

            //Act
            Assert.Throws<InvalidOperationException>(() => CreateRuleEngine(ruleSet));

            Rule BuildRule()
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
                    new CodeThisReferenceExpression(),
                    "SetMoreValues",
                    new CodeExpression[]
                    {
                        new CodePrimitiveExpression("AAA"),
                        new CodePrimitiveExpression("BBB"),
                        new CodePrimitiveExpression("CCC")
                    }
                );

                return new Rule("Rule1")
                {
                    Condition = new RuleExpressionCondition(ruleStateTest),
                    ThenActions = { new RuleStatementAction(methodInvoke) }
                };
            }
        }

        [Fact]
        public void Test_Rule_constructor_invocation_with_too_many_parameters_throws_InvalidOperationException()
        {
            //Arrange
            RuleSet ruleSet = new RuleSet { Name = "MyRuleSet", ChainingBehavior = RuleChainingBehavior.Full };
            ruleSet.Rules.Add(BuildRule());

            //Act
            Assert.Throws<InvalidOperationException>(() => CreateRuleEngine(ruleSet));

            Rule BuildRule()
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
                    new CodeThisReferenceExpression(),
                    "SetValues",
                    new CodeObjectCreateExpression
                    (
                        new CodeTypeReference("SampleFlow.YetAnotherEntity"),
                        new CodeExpression[]
                        {
                            new CodePrimitiveExpression("AAA"),
                            new CodePrimitiveExpression("BBB"),
                            new CodePrimitiveExpression("CCC")
                        }
                    )
                );

                return new Rule("Rule1")
                {
                    Condition = new RuleExpressionCondition(ruleStateTest),
                    ThenActions = { new RuleStatementAction(methodInvoke) }
                };
            }
        }

        [Fact]
        public void Test_Rule_method_invocation_with_missing_optional_parameters_succeeds()
        {
            //Arrange
            RuleSet ruleSet = new RuleSet { Name = "MyRuleSet", ChainingBehavior = RuleChainingBehavior.Full };
            ruleSet.Rules.Add(BuildRule());
            RuleEngine ruleEngine = CreateRuleEngine(ruleSet);
            SampleFlow.FlowEntity entity = new SampleFlow.FlowEntity
            {
                State = "NC"
            };

            //Act
            ruleEngine.Execute(entity);

            //Assert
            Assert.Equal("AAA", entity.FirstValue);
            Assert.Equal("BBB", entity.SecondValue);
            Assert.Equal("", entity.ThirdValue);
            Assert.Equal(0, entity.FourthValue);
            Assert.Null(entity.TheParams);

            Rule BuildRule()
            {
                // define first predicate: this.State == "MA"
                CodeBinaryOperatorExpression ruleStateTest = new CodeBinaryOperatorExpression
                {
                    Left = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "State"),
                    Operator = CodeBinaryOperatorType.ValueEquality,
                    Right = new CodePrimitiveExpression("NC")
                };

                //action SampleFlow.FlowEntity.SetDefaultState("NH")
                CodeMethodInvokeExpression methodInvoke = new CodeMethodInvokeExpression
                (
                    new CodeThisReferenceExpression(),
                    "SetValues",
                    new CodeExpression[]
                    {
                        new CodePrimitiveExpression("AAA"),
                        new CodePrimitiveExpression("BBB")
                    }
                );

                return new Rule("Rule1")
                {
                    Condition = new RuleExpressionCondition(ruleStateTest),
                    ThenActions = { new RuleStatementAction(methodInvoke) }
                };
            }
        }

        [Fact]
        public void Test_Rule_constructor_invocation_with_missing_optional_parameters_succeeds()
        {
            //Arrange
            RuleSet ruleSet = new RuleSet { Name = "MyRuleSet", ChainingBehavior = RuleChainingBehavior.Full };
            ruleSet.Rules.Add(BuildRule());
            RuleEngine ruleEngine = CreateRuleEngine(ruleSet);
            SampleFlow.FlowEntity entity = new SampleFlow.FlowEntity
            {
                State = "NC"
            };

            //Act
            ruleEngine.Execute(entity);

            //Assert
            Assert.Equal("AAA", entity.FirstValue);
            Assert.Equal("BBB", entity.SecondValue);
            Assert.Equal("", entity.ThirdValue);
            Assert.Equal(0, entity.FourthValue);
            Assert.Null(entity.TheParams);

            Rule BuildRule()
            {
                // define first predicate: this.State == "MA"
                CodeBinaryOperatorExpression ruleStateTest = new CodeBinaryOperatorExpression
                {
                    Left = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "State"),
                    Operator = CodeBinaryOperatorType.ValueEquality,
                    Right = new CodePrimitiveExpression("NC")
                };

                //action SampleFlow.FlowEntity.SetDefaultState("NH")
                CodeMethodInvokeExpression methodInvoke = new CodeMethodInvokeExpression
                (
                    new CodeThisReferenceExpression(),
                    "SetValues",
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

                return new Rule("Rule1")
                {
                    Condition = new RuleExpressionCondition(ruleStateTest),
                    ThenActions = { new RuleStatementAction(methodInvoke) }
                };
            }
        }

        [Fact]
        public void Test_Rule_method_invocation_with_missing_optional_parameters_succeeds_without_params_argument()
        {
            //Arrange
            RuleSet ruleSet = new RuleSet { Name = "MyRuleSet", ChainingBehavior = RuleChainingBehavior.Full };
            ruleSet.Rules.Add(BuildRule());
            RuleEngine ruleEngine = CreateRuleEngine(ruleSet);
            SampleFlow.FlowEntity entity = new SampleFlow.FlowEntity
            {
                State = "NC"
            };

            //Act
            ruleEngine.Execute(entity);

            //Assert
            Assert.Equal("AAA", entity.FirstValue);
            Assert.Equal("BBB", entity.SecondValue);
            Assert.Equal("", entity.ThirdValue);
            Assert.Equal(0, entity.FourthValue);

            Rule BuildRule()
            {
                // define first predicate: this.State == "MA"
                CodeBinaryOperatorExpression ruleStateTest = new CodeBinaryOperatorExpression
                {
                    Left = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "State"),
                    Operator = CodeBinaryOperatorType.ValueEquality,
                    Right = new CodePrimitiveExpression("NC")
                };

                //action SampleFlow.FlowEntity.SetDefaultState("NH")
                CodeMethodInvokeExpression methodInvoke = new CodeMethodInvokeExpression
                (
                    new CodeThisReferenceExpression(),
                    "SetValuesWithoutParams",
                    new CodeExpression[]
                    {
                        new CodePrimitiveExpression("AAA"),
                        new CodePrimitiveExpression("BBB")
                    }
                );

                return new Rule("Rule1")
                {
                    Condition = new RuleExpressionCondition(ruleStateTest),
                    ThenActions = { new RuleStatementAction(methodInvoke) }
                };
            }
        }

        [Fact]
        public void Test_Rule_constructor_invocation_with_missing_optional_parameters_succeeds_without_params_argument()
        {
            //Arrange
            RuleSet ruleSet = new RuleSet { Name = "MyRuleSet", ChainingBehavior = RuleChainingBehavior.Full };
            ruleSet.Rules.Add(BuildRule());
            RuleEngine ruleEngine = CreateRuleEngine(ruleSet);
            SampleFlow.FlowEntity entity = new SampleFlow.FlowEntity
            {
                State = "NC"
            };

            //Act
            ruleEngine.Execute(entity);

            //Assert
            Assert.Equal("AAA", entity.FirstValue);
            Assert.Equal("BBB", entity.SecondValue);
            Assert.Equal("", entity.ThirdValue);
            Assert.Equal(0, entity.FourthValue);

            Rule BuildRule()
            {
                // define first predicate: this.State == "MA"
                CodeBinaryOperatorExpression ruleStateTest = new CodeBinaryOperatorExpression
                {
                    Left = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "State"),
                    Operator = CodeBinaryOperatorType.ValueEquality,
                    Right = new CodePrimitiveExpression("NC")
                };

                //action SampleFlow.FlowEntity.SetDefaultState("NH")
                CodeMethodInvokeExpression methodInvoke = new CodeMethodInvokeExpression
                (
                    new CodeThisReferenceExpression(),
                    "SetValues",
                    new CodeObjectCreateExpression
                    (
                        new CodeTypeReference("SampleFlow.EntityWithoutParams"),
                        new CodeExpression[]
                        {
                            new CodePrimitiveExpression("AAA"),
                            new CodePrimitiveExpression("BBB")
                        }
                    )
                );

                return new Rule("Rule1")
                {
                    Condition = new RuleExpressionCondition(ruleStateTest),
                    ThenActions = { new RuleStatementAction(methodInvoke) }
                };
            }
        }

        private RuleEngine CreateRuleEngine(RuleSet ruleSet)
        {
            return new RuleEngine(ruleSet, Helper.GetValidation(ruleSet, typeof(SampleFlow.FlowEntity)));
        }
    }
}
