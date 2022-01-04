using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Backups.Interfaces;

namespace Backups
{
    public class SplitStorage : IAlgorithm
    {
        private List<string> _filesToArchivatePaths;
        private List<string> _archivedFiles;
        private IArchiver _archiver;

        public SplitStorage(List<string> filesToArchivatePaths, IArchiver archiver)
        {
            _filesToArchivatePaths = filesToArchivatePaths;
            _archiver = archiver;
            _archivedFiles = new List<string>();
        }

        public List<string> GetStorages()
        {
            return _archivedFiles;
        }

        public void Archive(uint numberOfRestorePoint, string outputDirectoryPath)
        {
            _archiver.Archive(numberOfRestorePoint, outputDirectoryPath, _archivedFiles, _filesToArchivatePaths);
        }

        public IEnumerable<string> GetArchivedFiles()
        {
            return _archivedFiles;
        }
    }
}