using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Backups.Interfaces;

namespace Backups.Archiver
{
    public class TestSplitArchiver : Interfaces.IArchiver
    {
        public void Archive(uint numberOfRestorePoint, string outputDirectoryPath, List<string> archivedFiles, List<string> filesToArchivatePaths)
        {
            string pathToStore = GetPathToStore(numberOfRestorePoint, outputDirectoryPath);
            int fileNumber = 1;
            foreach (string fileName in filesToArchivatePaths.Select(ArchivePathConverter.ConvertPathToName))
            {
                archivedFiles.Add(GetArchiveFileName(pathToStore, fileName, fileNumber));
                fileNumber++;
            }
        }

        private static string GetArchiveFileName(string pathToStore, string fileName, int fileNumber)
        {
            return pathToStore + Path.DirectorySeparatorChar + fileName + fileNumber + "_" + ArchivePathConverter.ConvertDateTimeNowToString() + ".zip";
        }

        private static string GetPathToStore(uint numberOfRestorePoint, string outputDirectoryPath)
        {
            return outputDirectoryPath + Path.DirectorySeparatorChar + "RestorePoint" + numberOfRestorePoint;
        }
    }
}