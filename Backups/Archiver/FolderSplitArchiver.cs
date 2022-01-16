using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups.Interfaces;

namespace Backups.Archiver
{
    public class FolderSplitArchiver : IArchiver
    {
        public void Archive(uint numberOfRestorePoint, string outputDirectoryPath, List<string> archivedFiles, List<string> filesToArchivatePaths)
        {
            string pathToStore = outputDirectoryPath + Path.DirectorySeparatorChar + "RestorePoint" +
                                 numberOfRestorePoint;
            Directory.CreateDirectory(pathToStore);
            int fileNumber = 1;
            foreach (string filePath in filesToArchivatePaths)
            {
                string fileName = ArchivePathConverter.ConvertPathToName(filePath);
                string pathToTempFolder = GetPathToTempFolder(pathToStore, fileName, fileNumber);
                Directory.CreateDirectory(pathToTempFolder);
                File.Copy(filePath, pathToTempFolder + Path.DirectorySeparatorChar + fileName);
                ZipFile.CreateFromDirectory(pathToTempFolder, pathToTempFolder + ".zip");
                File.Move(pathToTempFolder + ".zip", GetDestinationFileName(pathToStore, fileName, fileNumber), true);
                archivedFiles.Add(GetArchiveFileName(pathToStore, fileName, fileNumber));
                Directory.Delete(pathToTempFolder, true);
                fileNumber++;
            }
        }

        private static string GetPathToTempFolder(string pathToStore, string fileName, int fileNumber)
        {
            return pathToStore + "_" + fileName + fileNumber + "_" + ArchivePathConverter.ConvertDateTimeNowToString();
        }

        private static string GetArchiveFileName(string pathToStore, string fileName, int fileNumber)
        {
            return pathToStore + Path.DirectorySeparatorChar + fileName + fileNumber + "_" + ArchivePathConverter.ConvertDateTimeNowToString() + ".zip";
        }

        private static string GetDestinationFileName(string pathToStore, string fileName, int fileNumber)
        {
            return pathToStore + Path.DirectorySeparatorChar + fileName + fileNumber + "_" + ArchivePathConverter.ConvertDateTimeNowToString() + ".zip";
        }
    }
}