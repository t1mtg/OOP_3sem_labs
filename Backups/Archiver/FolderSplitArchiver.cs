using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups.Interfaces;

namespace Backups.Archiver
{
    public class FolderSplitArchiver : Interfaces.Archiver
    {
        public override void Archive(uint numberOfRestorePoint, string outputDirectoryPath, List<string> archivedFiles, List<string> filesToArchivatePaths)
        {
            string pathToStore = outputDirectoryPath + Path.DirectorySeparatorChar + "RestorePoint" +
                                 numberOfRestorePoint;
            Directory.CreateDirectory(pathToStore);
            int fileNumber = 1;
            foreach (string filePath in filesToArchivatePaths)
            {
                string fileName = PathConvertToName(filePath);
                string pathToTempFolder = pathToStore + "_" + fileName + fileNumber + "_" + DateTimeNowToString();
                Directory.CreateDirectory(pathToTempFolder);
                File.Copy(filePath, pathToTempFolder + Path.DirectorySeparatorChar + fileName);
                ZipFile.CreateFromDirectory(pathToTempFolder, pathToTempFolder + ".zip");
                File.Move(pathToTempFolder + ".zip", pathToStore + Path.DirectorySeparatorChar + fileName + fileNumber + "_" + DateTimeNowToString() + ".zip", true);
                archivedFiles.Add(pathToStore + Path.DirectorySeparatorChar + fileName + fileNumber + "_" + DateTimeNowToString() + ".zip");
                Directory.Delete(pathToTempFolder, true);
                fileNumber++;
            }
        }
    }
}