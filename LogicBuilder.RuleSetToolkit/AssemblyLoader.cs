using LogicBuilder.RuleSetToolkit.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Security;

namespace LogicBuilder.RuleSetToolkit
{
    internal class AssemblyLoader
    {
        internal AssemblyLoader(string assemblyFullName, string[] paths, ToolkitAssemblyLoadContext assemblyLoadContext)
        {
            this.assemblyFullName = assemblyFullName;
            this.paths = paths;
            this.assemblyLoadContext = assemblyLoadContext;
        }

        #region Constants
        private const string DOTDLL = ".DLL";
        private const string COMMA = ",";
        #endregion Constants

        #region Variables
        private string assemblyFullName;
        private string[] paths;
        private ToolkitAssemblyLoadContext assemblyLoadContext;
        #endregion Variables

        #region Properties
        #endregion Properties

        #region Methods
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
                throw new ToolkitException(ex.Message, ex);
            }
            catch (SecurityException ex)
            {
                throw new ToolkitException(ex.Message, ex);
            }
            catch (FileLoadException ex)
            {
                throw new ToolkitException(ex.Message, ex);
            }
            catch (FileNotFoundException ex)
            {
                throw new ToolkitException(ex.Message, ex);
            }
            catch (TypeLoadException ex)
            {
                throw new ToolkitException(ex.Message, ex);
            }
            catch (ReflectionTypeLoadException ex)
            {
                throw new ToolkitException(ex.Message, ex);
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
                    assembly = assemblyLoadContext.LoadFromFileStream(this.assemblyFullName);
            }
            catch (FileLoadException ex)
            {
                throw new ToolkitException(ex.Message, ex);
            }
            catch (ArgumentNullException ex)
            {
                throw new ToolkitException(ex.Message, ex);
            }
            catch (BadImageFormatException ex)
            {
                throw new ToolkitException(ex.Message, ex);
            }
            catch (FileNotFoundException ex)
            {
                throw new ToolkitException(ex.Message, ex);
            }

            return assembly;
        }

        internal static Type[] GetTypes(Assembly assembly)
        {
            List<Type> types = new List<Type>();
            try
            {
                types.AddRange(typeof(string).Assembly.GetTypes());
                types.AddRange(assembly.GetTypes());
            }
            catch (ReflectionTypeLoadException e)
            {
                types.AddRange(e.Types);
            }
            catch (ArgumentException ex)
            {
                throw new ToolkitException(ex.Message, ex);
            }
            catch (SecurityException ex)
            {
                throw new ToolkitException(ex.Message, ex);
            }
            catch (FileLoadException ex)
            {
                throw new ToolkitException(ex.Message, ex);
            }
            catch (FileNotFoundException ex)
            {
                throw new ToolkitException(ex.Message, ex);
            }
            catch (TypeLoadException ex)
            {
                throw new ToolkitException(ex.Message, ex);
            }

            return types.ToArray();
        }

        /// <summary>
        /// Returns the assembly
        /// </summary>
        /// <returns></returns>
        internal Assembly LoadAssembly(AssemblyName assemblyName)
        {
            if (assemblyName == null)
                return null;

            Assembly assembly;
            LinkedList<string> path = new LinkedList<string>(this.GetPaths());
            try
            {
                string name = assemblyName.FullName.Contains(COMMA) ? assemblyName.FullName.Substring(0, assemblyName.FullName.IndexOf(COMMA, StringComparison.Ordinal)) : assemblyName.FullName;
                assembly = LoadAssembly(path.First, string.Concat(name, DOTDLL)) ?? this.assemblyLoadContext.LoadFromAssemblyName(assemblyName);
            }
            catch (IOException ex)
            {
                throw new ToolkitException(ex.Message, ex);
            }
            catch (ArgumentException ex)
            {
                throw new ToolkitException(ex.Message, ex);
            }
            catch (BadImageFormatException ex)
            {
                throw new ToolkitException(ex.Message, ex);
            }
            catch (SecurityException ex)
            {
                throw new ToolkitException(ex.Message, ex);
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
                string path = Functions.RemoveTrailingPathSeparator(Functions.FilePath(this.assemblyFullName));
                pathsDictionary.Add(path.ToLowerInvariant(), path);
            }

            if (this.paths != null && this.paths.Length > 0)
            {
                pathsDictionary = this.paths.Aggregate(pathsDictionary, (dic, next) =>
                {
                    string newPath = Functions.RemoveTrailingPathSeparator(next);
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
                return assemblyLoadContext.LoadFromFileStream(fullName);
            else if (path.Next != null)
                return LoadAssembly(path.Next, file);
            else
                return null;
        }
        #endregion Private Methods
    }
}
