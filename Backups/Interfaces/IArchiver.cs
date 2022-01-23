using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Backups.Interfaces
{
    public interface IArchiver
    {
        public void Archive(uint numberOfRestorePoint, string outputDirectoryPath, List<string> archivedFiles, List<string> filesToArchivatePaths);
    }
}