using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BackupsExtra.Limits;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace BackupsExtra.Tests
{
    public class BackupsExtraTest
    {
        private BackupLogger _logger;
        private List<string> _files;
        private BackupJob _backupJob;
        private FileSystemRepository _repository;
        private Cleaner _cleaner;


        [SetUp]
        public void Setup()
        {
            _logger = new BackupLogger(LogDestination.File, LogDate.DatePrefix,
                @"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra\logs.txt");
            _files = new List<string>()
            {
                @"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra\test_files\first.txt",
                @"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra\test_files\second.txt",
                @"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra\test_files\third.txt",
            };
            _backupJob = new BackupJob(new JobObject(_files), _logger);
            _repository = new FileSystemRepository(_files);
        }

        /*[Test]
        public void RestoreFiles()
        {
            RestorePoint restorePoint = _backupJob
                .AddRestorePoint(@"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra", _repository,
                    new RestorePointCreationSettings(DateTime.Now, StorageTypeConfig.Split, FileSystemConfig.Tests));
            foreach (string file in _files)
            {
                File.Delete(file);
            }

            int filesCount = Directory
                .GetFiles(@"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra\test_files").Length;
            Assert.AreEqual(filesCount, 0);
            _backupJob.RestoreFilesFromBackup(restorePoint, RestoreLocation.Original);
            filesCount = Directory
                .GetFiles(@"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra\test_files").Length;
            Assert.AreEqual(filesCount, 3);
        }*/

        [Test]
        public void CleanRestorePointsAmountLimit()
        {
            _cleaner = new Cleaner(new AmountLimit(_backupJob.GetRestorePoints(), 2));
            RestorePoint firstRestorePoint = _backupJob
                .AddRestorePoint(@"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra", _repository,
                    new RestorePointCreationSettings(DateTime.Now, StorageTypeConfig.SplitStorage, FileSystemConfig.Tests));
            RestorePoint secondRestorePoint = _backupJob
                .AddRestorePoint(@"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra", _repository,
                    new RestorePointCreationSettings(DateTime.Now, StorageTypeConfig.SplitStorage, FileSystemConfig.Tests));
            RestorePoint thirdRestorePoint = _backupJob
                .AddRestorePoint(@"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra", _repository,
                    new RestorePointCreationSettings(DateTime.Now, StorageTypeConfig.SplitStorage, FileSystemConfig.Tests));
            Cleaner.Clean(_backupJob.GetRestorePoints(), FileSystemConfig.Tests);
            Assert.AreEqual(_backupJob.GetRestorePoints().Count, 2);
        }

        [Test]
        public void CleanRestorePointsDateLimit()
        {
            _cleaner = new Cleaner(new DateLimit(_backupJob.GetRestorePoints(), DateTime.Now.AddSeconds(2)));
            RestorePoint firstRestorePoint = _backupJob
                .AddRestorePoint(@"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra", _repository,
                    new RestorePointCreationSettings(DateTime.Now, StorageTypeConfig.SplitStorage, FileSystemConfig.Tests));
            System.Threading.Thread.Sleep(3000);
            RestorePoint secondRestorePoint = _backupJob
                .AddRestorePoint(@"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra", _repository,
                    new RestorePointCreationSettings(DateTime.Now, StorageTypeConfig.SplitStorage, FileSystemConfig.Tests));
            Cleaner.Clean(_backupJob.GetRestorePoints(), FileSystemConfig.Tests);
            Assert.AreEqual(_backupJob.GetRestorePoints().Count, 1);
        }

        [Test] 
        public void CleanRestorePointsHybridLimitAll()
        {
            _cleaner = new Cleaner(new HybridLimit(
                new DateLimit(_backupJob.GetRestorePoints(), DateTime.Now.AddSeconds(5)),
                new AmountLimit(_backupJob.GetRestorePoints(), 2), HybridType.All));
            RestorePoint firstRestorePoint = _backupJob
                .AddRestorePoint(@"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra", _repository,
                    new RestorePointCreationSettings(DateTime.Now, StorageTypeConfig.SplitStorage, FileSystemConfig.Tests));
            System.Threading.Thread.Sleep(5000);
            RestorePoint secondRestorePoint = _backupJob
                .AddRestorePoint(@"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra", _repository,
                    new RestorePointCreationSettings(DateTime.Now, StorageTypeConfig.SplitStorage, FileSystemConfig.Tests));
            RestorePoint thirdRestorePoint = _backupJob
                .AddRestorePoint(@"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra", _repository,
                    new RestorePointCreationSettings(DateTime.Now, StorageTypeConfig.SplitStorage, FileSystemConfig.Tests));
            Cleaner.Clean(_backupJob.GetRestorePoints(), FileSystemConfig.Tests);
            Assert.AreEqual(_backupJob.GetRestorePoints().Count, 2);
            
        }
        
        [Test] 
        public void CleanRestorePointsHybridLimitAny()
        {
            _cleaner = new Cleaner(new HybridLimit(
                new DateLimit(_backupJob.GetRestorePoints(), DateTime.Now.AddSeconds(2)),
                new AmountLimit(_backupJob.GetRestorePoints(), 2), HybridType.Any));
            RestorePoint firstRestorePoint = _backupJob
                .AddRestorePoint(@"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra", _repository,
                    new RestorePointCreationSettings(DateTime.Now, StorageTypeConfig.SplitStorage, FileSystemConfig.Tests));
            System.Threading.Thread.Sleep(5000);
            RestorePoint secondRestorePoint = _backupJob
                .AddRestorePoint(@"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra", _repository,
                    new RestorePointCreationSettings(DateTime.Now, StorageTypeConfig.SplitStorage, FileSystemConfig.Tests));
            RestorePoint thirdRestorePoint = _backupJob
                .AddRestorePoint(@"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra", _repository,
                    new RestorePointCreationSettings(DateTime.Now, StorageTypeConfig.SplitStorage, FileSystemConfig.Tests));
            RestorePoint fourthRestorePoint = _backupJob
                .AddRestorePoint(@"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra", _repository,
                    new RestorePointCreationSettings(DateTime.Now, StorageTypeConfig.SplitStorage, FileSystemConfig.Tests));
            Cleaner.Clean(_backupJob.GetRestorePoints(), FileSystemConfig.Tests);
            Assert.AreEqual(_backupJob.GetRestorePoints().Count, 2);
        }
        
        /*[Test] 
        public void MergeRestorePoints()
        {
            _cleaner = new Cleaner(new AmountLimit(_backupJob.GetRestorePoints(), 2));
            RestorePoint restorePoint0 = _backupJob
                .AddRestorePoint(@"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra", _repository,
                    new RestorePointCreationSettings(DateTime.Now, StorageTypeConfig.Split, FileSystemConfig.Tests));
            RestorePoint restorePoint1 = _backupJob
                .AddRestorePoint(@"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra", _repository,
                    new RestorePointCreationSettings(DateTime.Now, StorageTypeConfig.Split, FileSystemConfig.Tests));
            RestorePoint restorePoint2 = _backupJob
                .AddRestorePoint(@"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra", _repository,
                    new RestorePointCreationSettings(DateTime.Now, StorageTypeConfig.Split, FileSystemConfig.Folder));
            restorePoint1.RemoveLastFile();
            restorePoint2.RemoveLastFile();
            Cleaner.Clean(new List<RestorePoint>()
            {
                restorePoint0,
                restorePoint1,
                restorePoint2,
            });
            Assert.AreEqual(restorePoint1.Storages.Count, 2);
            Assert.AreEqual(_backupJob.GetRestorePoints().Count, 3);
        }
        */
        


    }
}