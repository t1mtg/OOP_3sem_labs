using System.Collections.Generic;
using System.IO;
using Backups.Interfaces;

namespace Backups.Archiver
{
    public class TestSingleArchiver : Interfaces.Archiver
    {
        public override void Archive(uint numberOfRestorePoint, string outputDirectoryPath, List<string> archivedFiles, List<string> filesToArchivatePaths)
        {
            string pathToStore = outputDirectoryPath + Path.DirectorySeparatorChar + "RestorePoint" + numberOfRestorePoint;
            archivedFiles.Add(pathToStore);
        }
    }
}