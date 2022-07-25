using LogicBuilder.Workflow.Activities.Rules;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace UnitTests.NetCore
{
    internal static class Helper
    {
        internal static RuleValidation GetValidation(RuleSet ruleSet, Type type)
        {
            RuleValidation ruleValidation = null;

            if (ruleSet == null)
                throw new InvalidOperationException(Resources.ruleSetCannotBeNull);

            ruleValidation = new RuleValidation(type);
            if (ruleValidation == null)
                throw new InvalidOperationException(Resources.ruleValidationCannotBeNull);

            if (!ruleSet.Validate(ruleValidation))
            {
                throw new InvalidOperationException
                (
                    string.Join
                    (
                        Environment.NewLine,
                        ruleValidation.Errors.Aggregate
                        (
                            new List<string> 
                            {
                                string.Format(CultureInfo.CurrentCulture, Resources.invalidRuleSetFormat, ruleSet.Name) 
                            }, 
                            (list, next) =>
                            {
                                list.Add(next.ErrorText);
                                return list;
                            }
                        )
                    )
                );
            }

            return ruleValidation;
        }
    }
}
