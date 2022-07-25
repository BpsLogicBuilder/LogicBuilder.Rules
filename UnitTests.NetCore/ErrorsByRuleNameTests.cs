using LogicBuilder.Workflow.Activities.Rules;
using System.CodeDom;
using Xunit;

namespace UnitTests.NetCore
{
    public class ErrorsByRuleNameTests
    {
        [Fact]
        public void ErrorsByRuleName_returns_expected_results()
        {
            //Arrange
            RuleSet ruleSet = CreateRuleSet();
            RuleValidation ruleValidation = new(typeof(SampleFlow.FlowEntity));

            //Act
            bool valid = ruleSet.Validate(ruleValidation);

            //Assert
            Assert.False(valid);
            Assert.Equal(3, ruleValidation.ErrorsByRuleName.Count);
            Assert.Equal(9, ruleValidation.ErrorsByRuleName["Rule0"].Count);
            Assert.Equal(1, ruleValidation.ErrorsByRuleName["Rule1"].Count);
            Assert.Equal(1, ruleValidation.ErrorsByRuleName["Rule2"].Count);
        }

        private static Rule Rule_set_literals()
        {
            CodePropertyReferenceExpression invariantCultureReference = new(new CodeTypeReferenceExpression(typeof(System.Globalization.CultureInfo)), "InvariantCulture");

            CodeBinaryOperatorExpression ruleStateTest = new()
            {
                Left = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "State"),
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = new CodePrimitiveExpression("TX")
            };

            CodePropertyReferenceExpression dClassRef = new(new CodeThisReferenceExpression(), "DClass2NotFound");

            CodeAssignStatement dateAction = new
            (
                dClassRef,
                new CodeMethodInvokeExpression
                    (
                        new CodeTypeReferenceExpression(typeof(System.DateTime)),
                        "Parse",
                        new CodeExpression[] { new CodePrimitiveExpression("01/03/2017"), invariantCultureReference }
                    )
            );

            CodeAssignStatement timeSpanAction = new
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
            CodeAssignStatement guidAction = new
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
            CodeAssignStatement decimalAction = new(dClassRef, new CodePrimitiveExpression(100.0012m));
            CodeAssignStatement decimalAction1 = new(dClassRef, new CodePrimitiveExpression(decimal.Parse("100.0012", System.Globalization.CultureInfo.InvariantCulture)));
            CodeAssignStatement charAction = new(dClassRef, new CodePrimitiveExpression(char.Parse("P")));
            CodeAssignStatement intAction = new(dClassRef, new CodePrimitiveExpression(100));
            CodeAssignStatement floatAction = new(dClassRef, new CodePrimitiveExpression(100.01f));
            CodeAssignStatement floatAction2 = new(dClassRef, new CodePrimitiveExpression(float.Parse("100.0012")));

            Rule rule0 = new("Rule0")
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

        private static Rule Rule_with_equals_condition_and_setter_action()
        {
            // define first predicate: this.State == "CT"
            CodeBinaryOperatorExpression ruleStateTest = new()
            {
                Left = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "StateNotFound"),
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = new CodePrimitiveExpression("CT")
            };

            //discount action this.Discount = 100
            CodeAssignStatement discountAction = new
            (
                new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Discount"),
                new CodePrimitiveExpression(100)
            );

            Rule rule1 = new("Rule1")
            {
                Condition = new RuleExpressionCondition(ruleStateTest)
            };
            rule1.ThenActions.Add(new RuleStatementAction(discountAction));

            return rule1;
        }

        private static Rule Rule_with_array_indexer_condition_and_setter_action()
        {
            //this.StringList[1, 1]
            CodeArrayIndexerExpression indexerExpression = new
            (
                new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "StringList"),
                new CodeExpression[] { new CodePrimitiveExpression(1), new CodePrimitiveExpression(1) }
            );

            //this.StringList[1, 1] == "A"
            CodeBinaryOperatorExpression stringIndexTest = new()
            {
                Left = indexerExpression,
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = new CodePrimitiveExpression("A")
            };

            //discount action this.Discount = 200
            CodeAssignStatement discountAction = new
            (
                new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "DiscountNotFound"),
                new CodePrimitiveExpression(200)
            );

            Rule rule2 = new("Rule2")
            {
                Condition = new RuleExpressionCondition(stringIndexTest)
            };
            rule2.ThenActions.Add(new RuleStatementAction(discountAction));

            return rule2;
        }

        private static RuleSet CreateRuleSet()
        {
            RuleSet ruleSet = new()
            {
                Name = "MyRuleSet",
                ChainingBehavior = RuleChainingBehavior.Full
            };

            ruleSet.Rules.Add(Rule_set_literals());
            ruleSet.Rules.Add(Rule_with_equals_condition_and_setter_action());
            ruleSet.Rules.Add(Rule_with_array_indexer_condition_and_setter_action());

            return ruleSet;
        }
    }
}
