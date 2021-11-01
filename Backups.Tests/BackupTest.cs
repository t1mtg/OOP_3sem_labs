using System.Collections.Generic;
using NUnit.Framework;

namespace Backups.Tests
{
    public class BackupTest
    {
        [Test]
        public void AddBackupToBackupJob()
        {
            var backup = new Backup();
            var objects = new List<string>
            {
                @"C:\Users\BaHo\Documents\GitHub\t1mtg\Backups\pic1.png",
                @"C:\Users\BaHo\Documents\GitHub\t1mtg\Backups\pic2.png"
            };

            var repository = new FileSystemRepository(objects);
            BackupJob backupJob = backup.CreateBackupJob(new JobObject(objects));
            backupJob.AddNewRestorePoint(Algorithm.Split, FileSystemConfig.Tests, @"C:\Users\BaHo\Documents\GitHub\t1mtg\Backups", repository);
            backupJob.RemoveObject(@"C:\Users\BaHo\Documents\GitHub\t1mtg\Backups\pic1.png");
            backupJob.AddNewRestorePoint(Algorithm.Split, FileSystemConfig.Tests, @"C:\Users\BaHo\Documents\GitHub\t1mtg\Backups", repository);
            Assert.AreEqual(backupJob.GetListOfStorages().Count , 3);
            Assert.AreEqual(backupJob.GetListOfRestorePoints().Count , 2);
        }
    }
}