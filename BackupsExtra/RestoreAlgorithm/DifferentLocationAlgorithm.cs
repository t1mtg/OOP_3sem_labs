using System.IO.Compression;
using BackupsExtra.Exceptions;

namespace BackupsExtra.RestoreAlgorithm
{
    public class DifferentLocationAlgorithm : IRestoreAlgorithm
    {
        public void RestoreFilesFromBackup(RestorePoint restorePoint, string restorePath)
        {
            if (restorePath == default)
            {
                throw new IncorrectPathException();
            }

            foreach (string path in restorePoint.Storages)
            {
                ZipFile.ExtractToDirectory(path, restorePath, true);
            }
        }
    }
}