namespace BackupsExtra.RestoreAlgorithm
{
    public interface IRestoreAlgorithm
    {
        public void RestoreFilesFromBackup(RestorePoint restorePoint, string restorePath = default);
    }
}