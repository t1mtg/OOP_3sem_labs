using System.Collections.Generic;

namespace Backups.Interfaces
{
    public abstract class Irepository
    {
        public abstract IEnumerable<string> GetStorages();
        public abstract List<string> Save(Algorithm algorithm, FileSystemConfig config, uint numberOfRestorePoints, string path);
    }
}