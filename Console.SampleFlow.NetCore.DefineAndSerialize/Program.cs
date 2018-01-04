using LogicBuilder.Workflow.Activities.Rules;
using LogicBuilder.Workflow.ComponentModel.Serialization;
using Microsoft.Extensions.Configuration;
using System.CodeDom;
using System.IO;

namespace Console.SampleFlow.NetCore.DefineAndSerialize
{
    class Program
    {
        static IConfigurationRoot config;

        static void Main(string[] args)
        {
            config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            CreateAndSerializeRuleSet();
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

        private static Rule Rule_with_equals_condition_and_setter_action()
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

        private static Rule Rule_with_array_indexer_condition_and_setter_action()
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


        private static Rule Rule_with_method_condition()
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


        private static Rule Rule_with_multiple_conditions()
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


        private static Rule Rule_with_static_method_action()
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


        private static Rule Rule_with_reference_method_action()
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


        private static Rule Rule_with_Cast_object_expression_in_then_action()
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


        private static Rule Rule_with_simple_CodeObjectCreateExpression_calling_a_constructor_in_then_action()
        {
            // define first predicate: this.State == "NC"
            CodeBinaryOperatorExpression ruleStateTest = new CodeBinaryOperatorExpression
            {
                Left = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "State"),
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = new CodePrimitiveExpression("NC")
            };

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

        private static Rule Rule_with_list_initialization()
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

        private static Rule Rule_with_child_and_granchild_reference()
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

        private static void CreateAndSerializeRuleSet()
        {
            RuleSet ruleSet = new RuleSet
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

            string rulesSetString = SerializeRules(ruleSet);
            using (System.IO.StreamWriter sr = new System.IO.StreamWriter(config.GetSection("ruleSetFile").Value, false, System.Text.Encoding.Unicode))
            {
                sr.Write(rulesSetString);
            }
        }

        private static string SerializeRules(object drs)
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
    }
}
