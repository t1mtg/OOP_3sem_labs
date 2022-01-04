using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Interfaces;

namespace Backups.Archiver
{
    public class TestSplitArchiver : IArchiver
    {
        public void Archive(uint numberOfRestorePoint, string outputDirectoryPath, List<string> archivedFiles, List<string> filesToArchivatePaths)
        {
            string pathToStore = outputDirectoryPath + Path.DirectorySeparatorChar + "RestorePoint" + numberOfRestorePoint;
            int fileNumber = 1;
            foreach (string fileName in filesToArchivatePaths.Select(filePath => filePath[(filePath.LastIndexOf(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal) + 1) ..]))
            {
                string dateTime = DateTime.Now.ToString().Replace('/', '.').Replace(':', '.');
                archivedFiles.Add(pathToStore + Path.DirectorySeparatorChar + fileName + fileNumber + "_" + dateTime + ".zip");
                fileNumber++;
            }
        }
    }
}