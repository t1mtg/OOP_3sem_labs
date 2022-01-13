using System.Collections.Generic;
using System.IO;

namespace BackupsExtra
{
    public class TestSingleArchiver : Archiver
    {
        public override void Archive(int numberOfRestorePoint, string outputDirectoryPath, List<string> archivedFiles, List<string> filesToArchivatePaths)
        {
            string pathToStore = outputDirectoryPath + Path.DirectorySeparatorChar + "RestorePoint" + numberOfRestorePoint;
            archivedFiles.Add(pathToStore);
        }
    }
}