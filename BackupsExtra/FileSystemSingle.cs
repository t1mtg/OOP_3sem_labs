using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace BackupsExtra
{
    public class FileSystemSingle
    {
        private List<string> _archivatedFiles = new List<string>();
        private List<string> _filesToArchivatePaths;

        public FileSystemSingle(List<string> filesToArchivatePaths)
        {
            _filesToArchivatePaths = filesToArchivatePaths;
        }

        public List<string> GetStorages()
        {
            return _archivatedFiles;
        }

        public void Archivate(FileSystemConfig fileSystemConf, int numberOfRestorePoint, string outputDirectoryPath)
        {
            switch (fileSystemConf)
            {
                case FileSystemConfig.Folder:
                    string pathToStore = outputDirectoryPath + Path.DirectorySeparatorChar + "RestorePoint" + numberOfRestorePoint;
                    Directory.CreateDirectory(pathToStore);
                    string dateTime = DateTime.Now.ToString().Replace('/', '.').Replace(':', '.');
                    string pathToTempFolder = pathToStore + "_" + dateTime;
                    Directory.CreateDirectory(pathToTempFolder);
                    foreach (string filePath in _filesToArchivatePaths)
                    {
                        string fileName = filePath[(filePath.LastIndexOf(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal) + 1) ..];
                        File.Copy(filePath, pathToTempFolder + Path.DirectorySeparatorChar + fileName);
                    }

                    ZipFile.CreateFromDirectory(pathToTempFolder, pathToTempFolder + ".zip");
                    File.Move(pathToTempFolder + ".zip", pathToStore + Path.DirectorySeparatorChar + pathToTempFolder[(pathToTempFolder.LastIndexOf(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal) + 1) ..] + ".zip");
                    _archivatedFiles.Add(pathToStore + Path.DirectorySeparatorChar +
                                       pathToTempFolder[
                                           (pathToTempFolder.LastIndexOf(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal) + 1) ..] +
                                       ".zip");
                    Directory.Delete(pathToTempFolder, true);
                    return;
                case FileSystemConfig.Tests:
                    pathToStore = outputDirectoryPath + Path.DirectorySeparatorChar + "RestorePoint" + numberOfRestorePoint;
                    _archivatedFiles.Add(pathToStore);
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fileSystemConf), fileSystemConf, null);
            }
        }
    }
}