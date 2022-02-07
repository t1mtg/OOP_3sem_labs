using System;
using System.IO;

namespace BackupsExtra.Archivers
{
    public class ArchivePathConverter
    {
        public static string ConvertPathToName(string path)
        {
            return path[(path.LastIndexOf(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal) + 1) ..];
        }

        public static string ConvertDateTimeNowToString()
        {
            return DateTime.Now.ToLongDateString();
        }
    }
}