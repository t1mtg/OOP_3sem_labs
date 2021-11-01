using System.ComponentModel.DataAnnotations;

namespace Backups
{
    public class Backup
    {
        private BackupJob _backupJob;

        public BackupJob CreateBackupJob(JobObject jobObject)
        {
            _backupJob = new BackupJob(jobObject);
            return _backupJob;
        }
    }
}