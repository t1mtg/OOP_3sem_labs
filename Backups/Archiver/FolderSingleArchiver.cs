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
            string pathToStore = outputDirectoryPath + Path.DirectorySeparatorChar + "RestorePoint" + numberOfRestorePoint;
            Directory.CreateDirectory(pathToStore);
            string dateTime = DateTime.Now.ToString(CultureInfo.InvariantCulture).Replace('/', '.').Replace(':', '.');
            string pathToTempFolder = pathToStore + "_" + dateTime;
            Directory.CreateDirectory(pathToTempFolder);
            foreach (string filePath in filesToArchivatePaths)
            {
                string fileName = filePath[(filePath.LastIndexOf(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal) + 1) ..];
                File.Copy(filePath, pathToTempFolder + Path.DirectorySeparatorChar + fileName);
            }

            ZipFile.CreateFromDirectory(pathToTempFolder, pathToTempFolder + ".zip");
            File.Move(pathToTempFolder + ".zip", pathToStore + Path.DirectorySeparatorChar + pathToTempFolder[(pathToTempFolder.LastIndexOf(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal) + 1) ..] + ".zip");
            archivedFiles.Add(pathToStore + Path.DirectorySeparatorChar +
                               pathToTempFolder[
                                   (pathToTempFolder.LastIndexOf(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal) + 1) ..] +
                               ".zip");
            Directory.Delete(pathToTempFolder, true);
        }
    }
}