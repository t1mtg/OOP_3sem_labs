using System.Collections.Generic;
namespace Backups
{
    internal class Program
    {
        private static void Main()
        {
            var list = new List<string>
            {
                @"C:\Users\BaHo\Documents\GitHub\t1mtg\Backups\pic1.png",
                @"C:\Users\BaHo\Documents\GitHub\t1mtg\Backups\pic2.png",
            };
            var backup = new Backup();
            BackupJob backupJob = backup.CreateBackupJob(new JobObject(list));
            var fileSystemRepository = new FileSystemRepository(list);
            backupJob.AddNewRestorePoint(Algorithm.SingleArchive, FileSystemConfig.Folder, @"C:\Users\BaHo\Documents\GitHub\t1mtg\Backups\res", fileSystemRepository);
            backupJob.AddNewRestorePoint(Algorithm.SingleArchive, FileSystemConfig.Folder, @"C:\Users\BaHo\Documents\GitHub\t1mtg\Backups\res", fileSystemRepository);
        }
    }
}
