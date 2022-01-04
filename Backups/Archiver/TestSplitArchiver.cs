using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Backups.Interfaces;

namespace Backups.Archiver
{
    public class TestSplitArchiver : Interfaces.Archiver
    {
        public override void Archive(uint numberOfRestorePoint, string outputDirectoryPath, List<string> archivedFiles, List<string> filesToArchivatePaths)
        {
            string pathToStore = outputDirectoryPath + Path.DirectorySeparatorChar + "RestorePoint" + numberOfRestorePoint;
            int fileNumber = 1;
            foreach (string fileName in filesToArchivatePaths.Select(PathConvertToName))
            {
                archivedFiles.Add(pathToStore + Path.DirectorySeparatorChar + fileName + fileNumber + "_" + DateTimeNowToString() + ".zip");
                fileNumber++;
            }
        }
    }
}