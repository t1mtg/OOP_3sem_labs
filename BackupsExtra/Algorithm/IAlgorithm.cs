using System.Collections.Generic;

namespace BackupsExtra
{
    public interface IAlgorithm
    {
        public void Archive(int numberOfRestorePoint, string outputDirectoryPath);
        public IEnumerable<string> GetArchivedFiles();

        public void DeleteStorages(List<string> storages);
    }
}