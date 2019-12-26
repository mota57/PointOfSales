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
            string workingDirectory = Directory.GetCurrentDirectory ();
            string projectDirectory = Directory.GetParent (workingDirectory).Parent.Parent.FullName;
            return projectDirectory;
        }

    }
}
