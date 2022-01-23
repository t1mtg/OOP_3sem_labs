using System.Collections.Generic;

namespace Backups.Interfaces
{
    public interface IAlgorithm
    {
        public void Archive(uint numberOfRestorePoint, string outputDirectoryPath);
        public IEnumerable<string> GetArchivedFiles();
    }
}