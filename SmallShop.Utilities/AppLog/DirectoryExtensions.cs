using System.Collections.Generic;
using System.IO;

namespace SmallShop.Utilities.Lib.AppLog
{
    internal static class DirectoryExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirPath">dir path</param>
        /// <param name="files">set of file paths</param>
        /// <param name="recursive">include subdirectory: true, otherwise: false</param>
        /// <returns></returns>
        public static long GetDirectoryLength(this string dirPath, ref List<string> files, bool recursive = false)
        {
            if (!Directory.Exists(dirPath))
                return 0;
            var di = new DirectoryInfo(dirPath);
            var fileInfos = di.GetFiles();
            long len = 0;
            foreach (var fileInfo in fileInfos)
            {
                files.Add(fileInfo.FullName);
                len += fileInfo.Length;
            }
            if (recursive)
            {
                var dis = di.GetDirectories();
                if (dis.Length > 0)
                {
                    foreach (var directoryInfo in dis)
                    {
                        len += GetDirectoryLength(directoryInfo.FullName, ref files, recursive);
                    }
                }
            }
            return len;
        }
    }
}
