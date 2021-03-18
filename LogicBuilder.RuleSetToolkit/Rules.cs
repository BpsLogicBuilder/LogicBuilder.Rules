using LogicBuilder.Workflow.Activities.Rules;
using LogicBuilder.Workflow.ComponentModel.Compiler;
using LogicBuilder.Workflow.ComponentModel.Serialization;
using LogicBuilder.RuleSetToolkit.Exceptions;
using LogicBuilder.RuleSetToolkit.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Reflection;

namespace LogicBuilder.RuleSetToolkit
{
    internal static class Rules
    {
        /// <summary>
        /// Returns a RuleSet given the fullFileName to the RuleSet file.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        internal static RuleSet LoadRuleSet(string fullPath)
        {
            RuleSet ruleSet = null;
            try
            {
                using (StreamReader inStream = new StreamReader(fullPath, Encoding.Unicode))
                    ruleSet = DeserializeRuleSet(inStream.ReadToEnd().UpdateStrongNameToNetFramework(), fullPath);
            }
            catch (ArgumentNullException ex)
            {
                throw new ToolkitException(ex.Message, ex);
            }
            catch (ArgumentException ex)
            {
                throw new ToolkitException(ex.Message, ex);
            }
            catch (IOException ex)
            {
                throw new ToolkitException(ex.Message, ex);
            }
            catch (NotSupportedException ex)
            {
                throw new ToolkitException(ex.Message, ex);
            }

            return ruleSet;
        }

        private static string UpdateStrongNameToNetFramework(this string ruleSetXml)
        {
            if (ruleSetXml == null) return null;

            ruleSetXml = Regex.Replace(ruleSetXml, AssemblyStrongNames.NETCORE_MATCH, AssemblyStrongNames.NETFRAMEWORK);
            ruleSetXml = Regex.Replace(ruleSetXml, AssemblyStrongNames.XAMARIN_MATCH, AssemblyStrongNames.NETFRAMEWORK);
            ruleSetXml = Regex.Replace(ruleSetXml, AssemblyStrongNames.NETNATIVE_MATCH, AssemblyStrongNames.NETFRAMEWORK);
            ruleSetXml = Regex.Replace(ruleSetXml, AssemblyStrongNames.CODEDOM_NETCORE_MATCH, AssemblyStrongNames.CODEDOM_NETFRAMEWORK);
            ruleSetXml = Regex.Replace(ruleSetXml, AssemblyStrongNames.CODEDOM_XAMARIN_MATCH, AssemblyStrongNames.CODEDOM_NETFRAMEWORK);
            ruleSetXml = Regex.Replace(ruleSetXml, AssemblyStrongNames.CODEDOM_NETNATIVE_MATCH, AssemblyStrongNames.CODEDOM_NETFRAMEWORK);

            return ruleSetXml;
        }

        private static string UpdateStrongNameByPlatForm(this string ruleSetXml, DotNetPlatForm platForm)
        {
            if (ruleSetXml == null) return null;
            if (platForm == DotNetPlatForm.NetFramework)
                return ruleSetXml;

            ruleSetXml = Regex.Replace(ruleSetXml, AssemblyStrongNames.NETFRAMEWORK_MATCH, StrongNames[platForm]);
            ruleSetXml = Regex.Replace(ruleSetXml, AssemblyStrongNames.CODEDOM_NETFRAMEWORK_MATCH, CodeDomStrongNames[platForm]);
            return ruleSetXml;
        }

        private static RuleSet DeserializeRuleSet(string ruleSetXml, string fullPath)
        {
            WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();
            if (!string.IsNullOrEmpty(ruleSetXml))
            {
                StringReader stringReader = new StringReader(ruleSetXml);
                XmlTextReader reader = new XmlTextReader(stringReader);
                RuleSet ruleSet = null;

                try
                {
                    ruleSet = serializer.Deserialize(reader) as RuleSet;
                }
                catch (WorkflowMarkupSerializationException ex)
                {
                    throw new ToolkitException(string.Format(CultureInfo.CurrentCulture, Resources.invalidRuleSetFormat, fullPath), ex);
                }

                return ruleSet;
            }
            else
            {
                return null;
            }
        }

        private static readonly IDictionary<DotNetPlatForm, string> StrongNames = new Dictionary<DotNetPlatForm, string>
        {
            { DotNetPlatForm.NetCore, AssemblyStrongNames.NETCORE },
            { DotNetPlatForm.NetFramework, AssemblyStrongNames.NETFRAMEWORK },
            { DotNetPlatForm.Xamarin, AssemblyStrongNames.XAMARIN },
            { DotNetPlatForm.NetNative, AssemblyStrongNames.NETNATIVE}
        };

        private static readonly IDictionary<DotNetPlatForm, string> CodeDomStrongNames = new Dictionary<DotNetPlatForm, string>
        {
            { DotNetPlatForm.NetCore, AssemblyStrongNames.CODEDOM_NETCORE },
            { DotNetPlatForm.NetFramework, AssemblyStrongNames.CODEDOM_NETFRAMEWORK },
            { DotNetPlatForm.Xamarin, AssemblyStrongNames.CODEDOM_XAMARIN },
            { DotNetPlatForm.NetNative, AssemblyStrongNames.CODEDOM_NETNATIVE}
        };

        internal static string SerializeRuleSet(object rules, DotNetPlatForm platForm)
        {
            StringBuilder ruleSet = new StringBuilder();
            WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();
            StringWriter stringWriter = new StringWriter(ruleSet, CultureInfo.InvariantCulture);
            XmlTextWriter writer = new XmlTextWriter(stringWriter);
            serializer.Serialize(writer, rules);
            writer.Flush();
            writer.Close();
            stringWriter.Flush();
            stringWriter.Close();
            return platForm == DotNetPlatForm.NetFramework
                ? ruleSet.ToString()
                : ruleSet.ToString().UpdateStrongNameByPlatForm(platForm);
        }

        /// <summary>
        /// Validates the rule set against the Activity Type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ruleSet"></param>
        /// <returns></returns>
        internal static List<string> ValidateRuleSet(Type type, RuleSet ruleSet, List<Assembly> referenceAssemblies)
        {
            List<string> validationErrors = new List<string>();
            RuleValidation ruleValidation;
            if (type == null)
                throw new InvalidOperationException(Resources.typeCannotBeNull);

            if (ruleSet == null)
                throw new InvalidOperationException(Resources.ruleSetCannotBeNull);

            ruleValidation = new RuleValidation(type, referenceAssemblies);

            if (ruleValidation == null)
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Resources.cannotValidateRuleSetFormat, ruleSet.Name));
                return validationErrors;
            }

            if (!ruleSet.Validate(ruleValidation))
            {
                foreach (ValidationError validationError in ruleValidation.Errors)
                    validationErrors.Add(validationError.ErrorText);
            }

            return validationErrors;
        }
    }
}
