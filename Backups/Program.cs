using System;
using System.Collections.Generic;
using Backups.Archiver;

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
            var archiver = new FolderSplitArchiver();
            backupJob.AddNewRestorePoint(new SplitStorage(list, archiver), @"C:\Users\BaHo\Documents\GitHub\t1mtg\Backups\res", fileSystemRepository);
            backupJob.AddNewRestorePoint(new SplitStorage(list, archiver), @"C:\Users\BaHo\Documents\GitHub\t1mtg\Backups\res", fileSystemRepository);
        }
    }
}
