using System.Collections.Generic;

namespace Backups.Interfaces
{
    public interface IRepository
    {
        public IEnumerable<string> GetStorages();
        public IEnumerable<string> Save(IAlgorithm algorithm, uint numberOfRestorePoints, string path);
    }
}