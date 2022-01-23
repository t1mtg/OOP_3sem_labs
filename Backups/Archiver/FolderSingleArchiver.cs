using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using Backups.Interfaces;

namespace Backups.Archiver
{
    public class FolderSingleArchiver : IArchiver
    {
        public void Archive(uint numberOfRestorePoint, string outputDirectoryPath, List<string> archivedFiles, List<string> filesToArchivatePaths)
        {
            string pathToStore = GetPathToStore(numberOfRestorePoint, outputDirectoryPath);
            Directory.CreateDirectory(pathToStore);
            string pathToTempFolder = GetPathToTempFolder(pathToStore);
            Directory.CreateDirectory(pathToTempFolder);
            foreach (string filePath in filesToArchivatePaths)
            {
                string fileName = ArchivePathConverter.ConvertPathToName(filePath);
                File.Copy(filePath, GetDestFileNameToCopy(pathToTempFolder, fileName));
            }

            ZipFile.CreateFromDirectory(pathToTempFolder, pathToTempFolder + ".zip");
            File.Move(pathToTempFolder + ".zip", GetDestinationFileName(pathToStore, pathToTempFolder));
            archivedFiles.Add(GetArchiveFileName(pathToStore, pathToTempFolder));
            Directory.Delete(pathToTempFolder, true);
        }

        private static string GetDestFileNameToCopy(string pathToTempFolder, string fileName)
        {
            return pathToTempFolder + Path.DirectorySeparatorChar + fileName;
        }

        private static string GetArchiveFileName(string pathToStore, string pathToTempFolder)
        {
            return pathToStore + Path.DirectorySeparatorChar + ArchivePathConverter.ConvertPathToName(pathToTempFolder) + ".zip";
        }

        private static string GetPathToTempFolder(string pathToStore)
        {
            return pathToStore + "_" + ArchivePathConverter.ConvertDateTimeNowToString();
        }

        private static string GetPathToStore(uint numberOfRestorePoint, string outputDirectoryPath)
        {
            return outputDirectoryPath + Path.DirectorySeparatorChar + "RestorePoint" + numberOfRestorePoint;
        }

        private static string GetDestinationFileName(string pathToStore, string pathToTempFolder)
        {
            return pathToStore + Path.DirectorySeparatorChar + ArchivePathConverter.ConvertPathToName(pathToTempFolder) + ".zip";
        }
    }
}