using System.IO;
using System.IO.Compression;
using System.Linq;

namespace BackupsExtra.RestoreAlgorithm
{
    public class OriginalLocationAlgorithm : IRestoreAlgorithm
    {
        public void RestoreFilesFromBackup(RestorePoint restorePoint, string restorePath = default)
        {
            restorePath = restorePoint.JobObject.Files[0];
            restorePath = restorePath[..restorePath.LastIndexOf(Path.DirectorySeparatorChar)];
            foreach (string path in restorePoint.Storages)
            {
                ZipFile.ExtractToDirectory(path, restorePath, true);
            }
        }
    }
}