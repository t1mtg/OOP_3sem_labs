using System;
using System.Collections.Generic;
using Backups.Interfaces;

namespace Backups
{
    public class FileSystemRepository : Irepository
    {
        private readonly List<string> _objects;
        private List<string> _storages;

        public FileSystemRepository(List<string> objects)
        {
            _objects = objects;
            _storages = new List<string>();
            Storages = _storages.AsReadOnly();
        }

        public IReadOnlyList<string> Storages { get; }

        public override IEnumerable<string> GetStorages()
        {
            return Storages;
        }

        public override List<string> Save(Algorithm algorithm, FileSystemConfig config, uint numberOfRestorePoints, string path)
        {
            switch (algorithm)
            {
                case Algorithm.Single:
                    var singleStorage = new SingleStorage(_objects);
                    singleStorage.Archive(config, numberOfRestorePoints, path);
                    _storages.AddRange(singleStorage.GetArchivedFiles());
                    return singleStorage.GetArchivedFiles();
                case Algorithm.Split:
                    var splitStorages = new SplitStorage(_objects);
                    splitStorages.Archive(config, numberOfRestorePoints, path);
                    _storages.AddRange(splitStorages.GetArchivedFiles());
                    return splitStorages.GetArchivedFiles();
                default:
                    throw new ArgumentOutOfRangeException(nameof(algorithm), algorithm, null);
            }
        }
    }
}