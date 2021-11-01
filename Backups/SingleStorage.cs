using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;

namespace Backups
{
    public class SingleStorage
    {
        private readonly List<string> _filePaths;
        private List<string> _archivedFiles = new List<string>();

        public SingleStorage(List<string> paths)
        {
            _filePaths = paths;
        }

        public List<string> GetArchivedFiles()
        {
            return _archivedFiles;
        }

        public void Archive(FileSystemConfig config, uint numberOfRestorePoints, string outputDirPath)
        {
            switch (config)
            {
                case FileSystemConfig.Folder:
                    string archivePath = outputDirPath + @"\" + "restorePoint" + numberOfRestorePoints;
                    Directory.CreateDirectory(archivePath);
                    outputDirPath = archivePath + "_" + DateTime.Now.ToString().Replace('/', '.').Replace(':', '.');
                    Directory.CreateDirectory(outputDirPath);
                    foreach (string filePath in _filePaths)
                    {
                        string fileName = filePath[(filePath.LastIndexOf(@"\", StringComparison.Ordinal) + 1) ..];
                        File.Copy(filePath, outputDirPath + @"\" + fileName);
                    }

                    ZipFile.CreateFromDirectory(outputDirPath, outputDirPath + ".zip");
                    File.Move(outputDirPath + ".zip", archivePath + @"\" + outputDirPath[(outputDirPath.LastIndexOf(@"\", StringComparison.Ordinal) + 1) ..] + ".zip");
                    _archivedFiles.Add(archivePath + @"\" +
                                     outputDirPath[(outputDirPath.LastIndexOf(@"\", StringComparison.Ordinal) + 1) ..] +
                                     ".zip");
                    Directory.Delete(outputDirPath, true);
                    return;
                case FileSystemConfig.Tests:
                    outputDirPath += @"\" + "restorePoint" + numberOfRestorePoints;
                    _archivedFiles.Add(outputDirPath);
                    return;
            }
        }
    }
}