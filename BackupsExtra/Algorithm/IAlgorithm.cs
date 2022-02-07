using System.Collections.Generic;

namespace BackupsExtra
{
    public interface IAlgorithm
    {
        public void Archive(uint numberOfRestorePoint, string outputDirectoryPath);
        public IEnumerable<string> GetArchivedFiles();

        public void DeleteStorages(List<string> storages);
    }
}