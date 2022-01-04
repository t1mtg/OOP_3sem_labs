using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using Backups.Interfaces;

namespace Backups.Archiver
{
    public class FolderSingleArchiver : Interfaces.Archiver
    {
        public override void Archive(uint numberOfRestorePoint, string outputDirectoryPath, List<string> archivedFiles, List<string> filesToArchivatePaths)
        {
            string pathToStore = outputDirectoryPath + Path.DirectorySeparatorChar + "RestorePoint" + numberOfRestorePoint;
            Directory.CreateDirectory(pathToStore);
            string pathToTempFolder = pathToStore + "_" + DateTimeNowToString();
            Directory.CreateDirectory(pathToTempFolder);
            foreach (string filePath in filesToArchivatePaths)
            {
                string fileName = PathConvertToName(filePath);
                File.Copy(filePath, pathToTempFolder + Path.DirectorySeparatorChar + fileName);
            }

            ZipFile.CreateFromDirectory(pathToTempFolder, pathToTempFolder + ".zip");
            File.Move(pathToTempFolder + ".zip", pathToStore + Path.DirectorySeparatorChar + PathConvertToName(pathToTempFolder) + ".zip");
            archivedFiles.Add(pathToStore + Path.DirectorySeparatorChar + PathConvertToName(pathToTempFolder) + ".zip");
            Directory.Delete(pathToTempFolder, true);
        }
    }
}