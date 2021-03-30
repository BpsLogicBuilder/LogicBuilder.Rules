using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace LogicBuilder.RuleSetToolkit
{
    internal class ToolkitAssemblyLoadContext : AssemblyLoadContext
    {
        private readonly string activityAssemblyFullName;
        private AssemblyDependencyResolver _resolver;

        public ToolkitAssemblyLoadContext(string activityAssemblyFullName) : base(isCollectible: true)
        {
            this.activityAssemblyFullName = activityAssemblyFullName;
            _resolver = new AssemblyDependencyResolver(this.activityAssemblyFullName);
        }

        protected override Assembly Load(AssemblyName name)
        {
            string assemblyPath = _resolver.ResolveAssemblyToPath(name);
            if (assemblyPath != null)
                return LoadFromFileStream(assemblyPath);

            return null;
        }

        internal Assembly LoadFromFileStream(string assemblyPath)
        {
            using (Stream assemblyStream = new FileStream(assemblyPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                return LoadFromStream(assemblyStream);
            }
        }
    }
}
