using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using Backups.Interfaces;

namespace Backups
{
    public class SingleStorage : IAlgorithm
    {
        private List<string> _archivedFiles = new List<string>();
        private List<string> _filesToArchivatePaths;
        private Interfaces.Archiver _archiver;

        public SingleStorage(List<string> filesToArchivatePaths, Interfaces.Archiver archiver)
        {
            _filesToArchivatePaths = filesToArchivatePaths;
            _archiver = archiver;
        }

        public IEnumerable<string> GetArchivedFiles()
        {
            return _archivedFiles;
        }

        public void Archive(uint numberOfRestorePoint, string outputDirectoryPath)
        {
            _archiver.Archive(numberOfRestorePoint, outputDirectoryPath, _archivedFiles, _filesToArchivatePaths);
        }
    }
}