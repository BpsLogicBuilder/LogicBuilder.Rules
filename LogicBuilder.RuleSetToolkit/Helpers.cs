using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LogicBuilder.RuleSetToolkit
{
    internal static class Helpers
    {
        public static List<Assembly> GetReferencedAssembliesRecursively(this Assembly assembly, AssemblyLoader assemblyLoader)
        {
            Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>
            {
                { assembly.FullName, assembly }
            };

            GetReferencedAssembliesRecursively(assembly, assemblies, assemblyLoader);

            return assemblies.Values.ToList();
        }

        public static void GetReferencedAssembliesRecursively(this Assembly assembly, Dictionary<string, Assembly> assemblies, AssemblyLoader assemblyLoader)
            => assembly.GetReferencedAssemblies()
                .ToList()
                .ForEach
                (
                    assemblyName =>
                    {
                        Assembly asm = null;
                        try
                        {
                            asm = assemblyLoader.LoadAssembly(assemblyName);
                        }
                        catch (Exception)
                        {
                        }

                        if (asm == null)
                            return;

                        if (!assemblies.ContainsKey(asm.FullName))
                        {
                            assemblies.Add(asm.FullName, asm);
                            GetReferencedAssembliesRecursively(asm, assemblies, assemblyLoader);
                        }
                    }
                );
    }
}
