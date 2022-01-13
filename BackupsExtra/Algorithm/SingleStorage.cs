using System.Collections.Generic;
using System.IO;

namespace BackupsExtra
{
    public class SingleStorage : IAlgorithm
    {
        private List<string> _archivedFiles = new List<string>();
        private List<string> _filesToArchivatePaths;
        private Archiver _archiver;

        public SingleStorage(List<string> filesToArchivatePaths, Archiver archiver)
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

        public void Archive(int numberOfRestorePoint, string outputDirectoryPath)
        {
            _archiver.Archive(numberOfRestorePoint, outputDirectoryPath, _archivedFiles, _filesToArchivatePaths);
        }
    }
}