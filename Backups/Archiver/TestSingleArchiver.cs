using System.Collections.Generic;
using System.IO;
using Backups.Interfaces;

namespace Backups.Archiver
{
    public class TestSingleArchiver : IArchiver
    {
        public void Archive(uint numberOfRestorePoint, string outputDirectoryPath, List<string> archivedFiles, List<string> filesToArchivatePaths)
        {
            string pathToStore = GetPathToStore(numberOfRestorePoint, outputDirectoryPath);
            archivedFiles.Add(pathToStore);
        }

        private static string GetPathToStore(uint numberOfRestorePoint, string outputDirectoryPath)
        {
            return outputDirectoryPath + Path.DirectorySeparatorChar + "RestorePoint" + numberOfRestorePoint;
        }
    }
}