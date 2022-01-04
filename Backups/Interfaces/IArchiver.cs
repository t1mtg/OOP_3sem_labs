using System.Collections.Generic;

namespace Backups.Interfaces
{
    public interface IArchiver
    {
       public void Archive(uint numberOfRestorePoint, string outputDirectoryPath, List<string> archivedFiles, List<string> filesToArchivatePaths);
    }
}