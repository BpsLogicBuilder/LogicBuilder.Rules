using System;
using System.Globalization;
using System.IO;

namespace LogicBuilder.RuleSetToolkit
{
    internal static class Functions
    {
        /// <summary>
        /// returns the file path given the full name
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        internal static string FilePath(string fileName)
        {
            return fileName.Substring(0, fileName.LastIndexOf(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture)));
        }

        internal static string RemoveTrailingPathSeparator(string path)
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
    }
}
