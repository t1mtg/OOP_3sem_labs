using System.Collections.Generic;
using BackupsExtra;

namespace BackupsExtra
{
    public interface IRepository
    {
        public IEnumerable<string> GetStorages();
        public IEnumerable<string> Save(IAlgorithm algorithm, int numberOfRestorePoints, string path);
    }
}