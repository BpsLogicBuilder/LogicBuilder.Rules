using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace LogicBuilder.Workflow.Activities.Rules
{
    internal class AssemblyLoader
    {
        internal AssemblyLoader(string assemblyFullName, string[] paths)
        {
            this.assemblyFullName = assemblyFullName;
            this.paths = paths;
            Initialize();
        }

        #region Constants
        private const string DOTDLL = ".DLL";
        private const string COMMA = ",";
        #endregion Constants

        #region Variables
        private string assemblyFullName;
        private string[] paths;
        #endregion Variables

        #region Properties
        #endregion Properties

        #region Methods
        private void Initialize()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        }

        /// <summary>
        /// Loads unresolved assembly i.e. referenced assembly not found in GAC or otherwise.
        /// This method searches the local directory of the assemblyFullName assembly.
        /// </summary>
        /// <param name="failedAssemblyStrongName"></param>
        /// <returns></returns>
        private Assembly ResolveAssembly(string failedAssemblyStrongName)
        {
            string assemblyName = failedAssemblyStrongName.Contains(COMMA) ? failedAssemblyStrongName.Substring(0, failedAssemblyStrongName.IndexOf(COMMA, StringComparison.Ordinal)) : failedAssemblyStrongName;
            LinkedList<string> path = new LinkedList<string>(this.GetPaths());
            try
            {
                return LoadAssembly(path.First, string.Concat(assemblyName, DOTDLL));
            }
            catch (FileLoadException ex)
            {
                throw new AssemblyLoaderException(ex.Message, ex);
            }
            catch (ArgumentNullException ex)
            {
                throw new AssemblyLoaderException(ex.Message, ex);
            }
            catch (BadImageFormatException ex)
            {
                throw new AssemblyLoaderException(ex.Message, ex);
            }
            catch (FileNotFoundException ex)
            {
                throw new AssemblyLoaderException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Given the fully qualified class name, returns the type form the assemblyFullName assembly
        /// </summary>
        /// <param name="className"></param>
        /// <param name="throwOnError"></param>
        internal static Type GetType(Assembly assembly, string className, bool throwOnError)
        {
            Type type = null;

            try
            {
                type = assembly.GetType(className, throwOnError);
            }
            catch (ArgumentException ex)
            {
                throw new AssemblyLoaderException(ex.Message, ex);
            }
            catch (SecurityException ex)
            {
                throw new AssemblyLoaderException(ex.Message, ex);
            }
            catch (FileLoadException ex)
            {
                throw new AssemblyLoaderException(ex.Message, ex);
            }
            catch (FileNotFoundException ex)
            {
                throw new AssemblyLoaderException(ex.Message, ex);
            }
            catch (TypeLoadException ex)
            {
                throw new AssemblyLoaderException(ex.Message, ex);
            }
            catch (ReflectionTypeLoadException ex)
            {
                throw new AssemblyLoaderException(ex.Message, ex);
            }

            return type;
        }

        /// <summary>
        /// Returns the assemblyFullName assembly
        /// </summary>
        /// <returns></returns>
        internal Assembly LoadAssembly()
        {
            if (String.IsNullOrEmpty(this.assemblyFullName))
                return null;

            Assembly assembly = null;
            try
            {
                if (File.Exists(this.assemblyFullName))
                    assembly = Assembly.LoadFile(this.assemblyFullName);
            }
            catch (FileLoadException ex)
            {
                throw new AssemblyLoaderException(ex.Message, ex);
            }
            catch (ArgumentNullException ex)
            {
                throw new AssemblyLoaderException(ex.Message, ex);
            }
            catch (BadImageFormatException ex)
            {
                throw new AssemblyLoaderException(ex.Message, ex);
            }
            catch (FileNotFoundException ex)
            {
                throw new AssemblyLoaderException(ex.Message, ex);
            }

            return assembly;
        }

        /// <summary>
        /// Returns the assembly
        /// </summary>
        /// <returns></returns>
        internal Assembly LoadAssembly(AssemblyName assemblyName)
        {
            if (assemblyName == null)
                return null;


            Assembly assembly = null;
            LinkedList<string> path = new LinkedList<string>(this.GetPaths());
            try
            {
                string name = assemblyName.FullName.Contains(COMMA) ? assemblyName.FullName.Substring(0, assemblyName.FullName.IndexOf(COMMA, StringComparison.Ordinal)) : assemblyName.FullName;
                assembly = LoadAssembly(path.First, string.Concat(name, DOTDLL)) ?? Assembly.Load(assemblyName.FullName);
            }
            catch (IOException ex)
            {
                throw new AssemblyLoaderException(ex.Message, ex);
            }
            catch (ArgumentException ex)
            {
                throw new AssemblyLoaderException(ex.Message, ex);
            }
            catch (BadImageFormatException ex)
            {
                throw new AssemblyLoaderException(ex.Message, ex);
            }
            catch (SecurityException ex)
            {
                throw new AssemblyLoaderException(ex.Message, ex);
            }

            return assembly;
        }

        #endregion Methods

        #region Private Methods
        private List<string> GetPaths()
        {
            Dictionary<string, string> pathsDictionary = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(this.assemblyFullName))
            {
                string path = RemoveTrailingPathSeparator(Path.GetDirectoryName(this.assemblyFullName));
                pathsDictionary.Add(path.ToLowerInvariant(), path);
            }

            if (this.paths != null && this.paths.Length > 0)
            {
                pathsDictionary = this.paths.Aggregate(pathsDictionary, (dic, next) =>
                {
                    string newPath = RemoveTrailingPathSeparator(next);
                    if (!dic.ContainsKey(newPath.ToLowerInvariant()))
                        dic.Add(newPath.ToLowerInvariant(), newPath);
                    return dic;
                });
            }

            return pathsDictionary.Values.ToList();
        }

        private Assembly LoadAssembly(LinkedListNode<string> path, string file)
        {
            string fullName = Path.Combine(path.Value, file);
            if (File.Exists(fullName))
                return Assembly.LoadFile(fullName);
            else if (path.Next != null)
                return LoadAssembly(path.Next, file);
            else
                return null;
        }

        private static string RemoveTrailingPathSeparator(string path)
        {
            return RemoveTrailingSeparator(path, Path.DirectorySeparatorChar.ToString());
        }

        private static string RemoveTrailingSeparator(string path, string separator)
        {
            if (path == null)
                return null;

            return path.Trim().EndsWith(separator, StringComparison.CurrentCulture)
                ? path.Trim().Substring(0, path.Trim().LastIndexOf(separator))
                : path.Trim();
        }
        #endregion Private Methods

        #region EventHandlers
        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (string.IsNullOrEmpty(this.assemblyFullName))
                return null;
            else
                return ResolveAssembly(args.Name);
        }
        #endregion EventHandlers
    }
}
