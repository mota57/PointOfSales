using System;
using System.Collections.Generic;
using System.IO;

namespace TablePlugin
{
    public static class UtilHelper
    {
        public static string GetFilePath (string fileName) {
            if (fileName.IsBlank ()) throw new ArgumentNullException (nameof (fileName));
            return Path.Combine (GetDirectoryProject (), fileName);
        }

        public static string GetDirectoryProject () {

            var parent = Directory.GetParent(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"))).Parent;
            string workingDirectory = Path.Combine(parent.FullName, "TablePlugin");
            return workingDirectory;
        }

    }
}
