using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace BackupsExtra
{
    public abstract class Archiver
    {
        public abstract void Archive(int numberOfRestorePoint, string outputDirectoryPath, List<string> archivedFiles, List<string> filesToArchivatePaths);

        protected static string PathConvertToName(string path)
        {
            return path[(path.LastIndexOf(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal) + 1) ..];
        }

        protected static string DateTimeNowToString()
        {
            return DateTime.Now.ToString(CultureInfo.InvariantCulture).Replace('/', '.').Replace(':', '.');
        }
    }
}