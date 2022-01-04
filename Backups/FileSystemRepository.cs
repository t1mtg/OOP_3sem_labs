using System;
using System.Collections.Generic;
using Backups.Interfaces;

namespace Backups
{
    public class FileSystemRepository : IRepository
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

        public IEnumerable<string> GetStorages()
        {
            return Storages;
        }

        public IEnumerable<string> Save(IAlgorithm algorithm, uint numberOfRestorePoints, string path)
        {
            algorithm.Archive(numberOfRestorePoints, path);
            _storages.AddRange(algorithm.GetArchivedFiles());
            return algorithm.GetArchivedFiles();
        }
    }
}