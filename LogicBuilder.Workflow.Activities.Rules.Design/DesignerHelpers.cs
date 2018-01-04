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

        static internal bool IsNetFrameworkAssembly(this Assembly assembly)
        {
            if (!(assembly.GetCustomAttribute<TargetFrameworkAttribute>() is TargetFrameworkAttribute attribute))
                return false;

            return attribute.FrameworkName.Contains(".NETFramework");
        }

        public static Type[] GetTypes(this Assembly root, string[] assemblyPaths)
        {
            if (root == null)
                return new Type[] { };
            //need an assembly loader for non-.NetFramework library
            //so that runtime types i.e. string, value types etc. can be loaded.
            AssemblyLoader assemblyLoader = new AssemblyLoader(root.Location, assemblyPaths);

            List<Type> types = new List<Type>();
            try
            {
                types.AddRange(root.GetTypes());
            }
            catch (ReflectionTypeLoadException e)
            {
                // problems loading all the types, take what we can get
                foreach (Type type in e.Types)
                    if (type != null)
                        types.Add(type);
            }
            foreach (Assembly a in root.GetReferencedAssemblies(assemblyLoader))
            {
                try
                {
                    types.AddRange(a.GetTypes());
                }
                catch (ReflectionTypeLoadException e)
                {
                    // problems loading all the types, take what we can get
                    foreach (Type type in e.Types)
                        if (type != null)
                            types.Add(type);
                }
            }
            return types.ToArray();
        }

        private static ICollection<Assembly> GetReferencedAssemblies(this Assembly root, AssemblyLoader assemblyLoader)
        {

            List<Assembly> references = null;
            // references is created on demand, does not include root
            if (references == null)
            {
                Dictionary<string, Assembly> dicAssemblies = new Dictionary<string, Assembly>();
                foreach (AssemblyName a in root.GetReferencedAssemblies())
                {
                    try
                    {
                        if (dicAssemblies.ContainsKey(a.FullName))
                            continue;

                        Assembly assem = Assembly.Load(a);

                        if (assem != null)
                            dicAssemblies.Add(a.FullName, assem);
                    }
                    catch (AssemblyLoaderException)
                    {
                    }
                }

                //needed at design time for non-.NetFramework assemblies
                if (assemblyLoader != null)
                {
                    foreach (AssemblyName a in root.GetReferencedAssemblies())
                    {
                        if (dicAssemblies.ContainsKey(a.FullName))
                            continue;

                        try
                        {
                            Assembly assem = assemblyLoader.LoadAssembly(a);
                            if (assem != null)
                                dicAssemblies.Add(a.FullName, assem);
                        }
                        catch (AssemblyLoaderException)
                        {
                        }
                    }

                    //Add mscorlib types if the assembly is not a .NetFramework assembly
                    if (!root.IsNetFrameworkAssembly() && !dicAssemblies.ContainsKey(typeof(string).Assembly.FullName))
                    {
                        dicAssemblies.Add(typeof(string).Assembly.FullName, typeof(string).Assembly);
                    }
                }

                references = dicAssemblies.Values.ToList();
            }
            return references;
        }
    }
}
