using System.Collections.Generic;
using System.IO;
using BackupsExtra.Archivers;

namespace BackupsExtra
{
    public class SingleStorage : IAlgorithm
    {
        private List<string> _archivedFiles = new List<string>();
        private List<string> _filesToArchivatePaths;
        private IArchiver _archiver;

        public SingleStorage(List<string> filesToArchivatePaths, IArchiver archiver)
        {
            _filesToArchivatePaths = filesToArchivatePaths;
            _archiver = archiver;
        }

        public IEnumerable<string> GetArchivedFiles()
        {
            return _archivedFiles;
        }

        public void DeleteStorages(List<string> storages)
        {
            foreach (string file in storages)
            {
                Directory.Delete(file[..file.LastIndexOf(Path.DirectorySeparatorChar)], true);
            }
        }

        public void Archive(uint numberOfRestorePoint, string outputDirectoryPath)
        {
            _archiver.Archive(numberOfRestorePoint, outputDirectoryPath, _archivedFiles, _filesToArchivatePaths);
        }
    }
}