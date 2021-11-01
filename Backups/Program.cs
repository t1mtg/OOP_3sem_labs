using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

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
            backupJob.AddNewRestorePoint(Algorithm.Single, FileSystemConfig.Folder, @"C:\Users\BaHo\Documents\GitHub\t1mtg\Backups\res", fileSystemRepository);
            backupJob.AddNewRestorePoint(Algorithm.Single, FileSystemConfig.Folder, @"C:\Users\BaHo\Documents\GitHub\t1mtg\Backups\res", fileSystemRepository);
        }
    }
}
