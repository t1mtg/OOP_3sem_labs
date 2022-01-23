using System;
using System.Collections.Generic;
using System.IO;
using BackupsExtra.Archivers;
using BackupsExtra.Cleaners;
using BackupsExtra.Limits;
using BackupsExtra.RestoreAlgorithm;
using NUnit.Framework;

namespace BackupsExtra
{
    internal class Program
    {
        private static void Main()
        {
            var logger = new BackupLogger(LogDestination.File, LogDate.DatePrefix, @"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra\logs.txt");
            var files = new List<string>()
            {
                @"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra\test_files\first.txt",
                @"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra\test_files\second.txt",
                @"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra\test_files\third.txt",
            };
            var backupJob = new BackupJob(new JobObject(files), logger);
            var repository = new FileSystemRepository(files);
            var archiver = new FolderSplitArchiver();
            RestorePoint restorePoint = backupJob.AddNewRestorePoint(new SplitStorage(files, archiver), @"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra\res", repository);
            foreach (string file in files)
            {
                File.Delete(file);
            }

            int filesCount = Directory
                .GetFiles(@"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra\test_files").Length;
            Assert.AreEqual(filesCount, 0);
            backupJob.RestoreFilesFromBackup(restorePoint, new OriginalLocationAlgorithm());
            filesCount = Directory
                .GetFiles(@"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra\test_files").Length;
            Assert.AreEqual(filesCount, 3);

            // merge test
            var cleaner = new Cleaner(new AmountLimit(backupJob.GetRestorePoints(), 3));
            RestorePoint restorePoint0 = backupJob.AddNewRestorePoint(new SplitStorage(files, archiver), @"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra\", repository);
            RestorePoint restorePoint1 = backupJob.AddNewRestorePoint(new SplitStorage(files, archiver), @"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra\", repository);
            RestorePoint restorePoint2 = backupJob.AddNewRestorePoint(new SplitStorage(files, archiver), @"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra\", repository);
            restorePoint1.RemoveLastFile();
            restorePoint2.RemoveLastFile();
            cleaner.Clean(new List<RestorePoint>()
            {
                restorePoint0,
                restorePoint1,
                restorePoint2,
            });
            Assert.AreEqual(restorePoint1.Storages.Count, 2);
            Assert.AreEqual(backupJob.GetRestorePoints().Count, 4);
        }
    }
}