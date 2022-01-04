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
                string fileName =
                    filePath[
                        (filePath.LastIndexOf(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal) +
                         1) ..];
                string dateTime = DateTime.Now.ToString().Replace('/', '.').Replace(':', '.');
                string pathToTempFolder = pathToStore + "_" + fileName + fileNumber + "_" + dateTime;
                Directory.CreateDirectory(pathToTempFolder);
                File.Copy(filePath, pathToTempFolder + Path.DirectorySeparatorChar + fileName);
                ZipFile.CreateFromDirectory(pathToTempFolder, pathToTempFolder + ".zip");
                File.Move(pathToTempFolder + ".zip", pathToStore + Path.DirectorySeparatorChar + fileName + fileNumber + "_" + dateTime + ".zip", true);
                archivedFiles.Add(pathToStore + Path.DirectorySeparatorChar + fileName + fileNumber + "_" + dateTime +
                                  ".zip");
                Directory.Delete(pathToTempFolder, true);
                fileNumber++;
            }
        }
    }
}