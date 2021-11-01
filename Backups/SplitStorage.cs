using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Backups
{
    public class SplitStorage
    {
        private readonly List<string> _filePaths;
        private List<string> _archivedFiles;

        public SplitStorage(List<string> paths)
        {
            _filePaths = paths;
            _archivedFiles = new List<string>();
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
                    uint fileNumber = 1;
                    foreach (string filePath in _filePaths)
                    {
                        Directory.CreateDirectory(outputDirPath);
                        string fileName = filePath[(filePath.LastIndexOf(@"\", StringComparison.Ordinal) + 1) ..];
                        File.Copy(filePath, outputDirPath + @"\" + fileName);
                        ZipFile.CreateFromDirectory(outputDirPath + @"\" + fileName, outputDirPath + @"\" + fileName + "_" + fileNumber + "_" + DateTime.Now + ".zip");
                        File.Move(outputDirPath + @"\" + fileName + DateTime.Now + ".zip", archivePath + @"\" + outputDirPath[(outputDirPath.LastIndexOf(@"\", StringComparison.Ordinal) + 1) ..] + ".zip");
                        _archivedFiles.Add(archivePath + @"\" +
                                           outputDirPath[
                                               (outputDirPath.LastIndexOf(@"\", StringComparison.Ordinal) + 1) ..] +
                                           ".zip");
                        Directory.Delete(outputDirPath, true);
                        fileNumber++;
                    }

                    return;
                case FileSystemConfig.Tests:
                    foreach (string filePath in _filePaths.Select(fp =>
                        fp[(fp.LastIndexOf("/", StringComparison.Ordinal) + 1) ..]))
                    {
                        _archivedFiles.Add(filePath);
                    }

                    return;
            }
        }
    }
}