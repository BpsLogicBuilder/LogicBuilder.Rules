using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Windows.Forms;

namespace LogicBuilder.Workflow.Activities.Rules.Design
{
    /// <summary>
    /// Summary description for DesignerHelpers.
    /// </summary>
    internal static class DesignerHelpers
    {
        internal static void DisplayError(string message, string messageBoxTitle)
        {
            MessageBox.Show(message, messageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, 0);
        }

        static internal string GetRulePreview(Rule rule)
        {
            StringBuilder rulePreview = new StringBuilder();

            if (rule != null)
            {
                rulePreview.Append("IF ");
                if (rule.Condition != null)
                    rulePreview.Append(rule.Condition.ToString() + " ");
                rulePreview.Append("THEN ");

                foreach (RuleAction action in rule.ThenActions)
                {
                    rulePreview.Append(action.ToString());
                    rulePreview.Append(' ');
                }

                if (rule.ElseActions.Count > 0)
                {
                    rulePreview.Append("ELSE ");
                    foreach (RuleAction action in rule.ElseActions)
                    {
                        rulePreview.Append(action.ToString());
                        rulePreview.Append(' ');
                    }
                }
            }

            return rulePreview.ToString();
        }

        static internal string GetRuleSetPreview(RuleSet ruleSet)
        {
            StringBuilder preview = new StringBuilder();
            bool first = true;
            if (ruleSet != null)
            {
                foreach (Rule rule in ruleSet.Rules)
                {
                    if (first)
                        first = false;
                    else
                        preview.Append("\n");

                    preview.Append(rule.Name);
                    preview.Append(": ");
                    preview.Append(DesignerHelpers.GetRulePreview(rule));
                }
            }

            return preview.ToString();
        }
    }
}
