using System;
using System.Collections.Generic;
using BackupsExtra.Archivers;
using BackupsExtra.Cleaners;
using BackupsExtra.Limits;
using NUnit.Framework;

namespace BackupsExtra.Tests
{
    public class BackupsExtraTest
    {
        private BackupLogger _logger;
        private List<string> _files;
        private BackupJob _backupJob;
        private FileSystemRepository _repository;
        private ICleaner _cleaner;
        private IArchiver _archiver;

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
            _archiver = new TestSplitArchiver();
        }

        [Test]
        public void CleanRestorePointsAmountLimit()
        {
            _cleaner = new TestCleaner(new AmountLimit(2));
            RestorePoint firstRestorePoint =
                _backupJob.AddNewRestorePoint(new SplitStorage(_files, _archiver),
                    @"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra\res", _repository);
            RestorePoint secondRestorePoint =
                _backupJob.AddNewRestorePoint(new SplitStorage(_files, _archiver),
                    @"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra\res", _repository);
            RestorePoint thirdRestorePoint =
                _backupJob.AddNewRestorePoint(new SplitStorage(_files, _archiver),
                    @"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra\res", _repository);
            _cleaner.Clean(_backupJob.GetRestorePoints());
            Assert.AreEqual(_backupJob.GetRestorePoints().Count, 2);
        }


        [Test]
        public void CleanRestorePointsDateLimit()
        {
            _cleaner = new TestCleaner(new DateLimit(DateTime.Now.AddSeconds(2)));
            RestorePoint firstRestorePoint =
                _backupJob.AddNewRestorePoint(new SplitStorage(_files, _archiver),
                    @"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra", _repository);
            System.Threading.Thread.Sleep(3000);
            RestorePoint secondRestorePoint =
                _backupJob.AddNewRestorePoint(new SplitStorage(_files, _archiver),
                    @"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra", _repository);
            _cleaner.Clean(_backupJob.GetRestorePoints());
            Assert.AreEqual(_backupJob.GetRestorePoints().Count, 1);
        }

        [Test]
        public void CleanRestorePointsHybridLimitAll()
        {
            _cleaner = new TestCleaner(new HybridLimit(
                new DateLimit(DateTime.Now.AddSeconds(1)),
                new AmountLimit(2), HybridType.All));
            RestorePoint firstRestorePoint =
                _backupJob.AddNewRestorePoint(new SplitStorage(_files, _archiver),
                    @"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra\res", _repository);
            System.Threading.Thread.Sleep(5000);    
            RestorePoint secondRestorePoint =
                _backupJob.AddNewRestorePoint(new SplitStorage(_files, _archiver),
                    @"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra\res", _repository);
            RestorePoint thirdRestorePoint =
                _backupJob.AddNewRestorePoint(new SplitStorage(_files, _archiver),
                    @"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra\res", _repository);
            RestorePoint fourthRestorePoint =
                _backupJob.AddNewRestorePoint(new SplitStorage(_files, _archiver),
                    @"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra\res", _repository);
            _cleaner.Clean(_backupJob.GetRestorePoints());
            _cleaner.Clean(_backupJob.GetRestorePoints());
            Assert.AreEqual(_backupJob.GetRestorePoints().Count, 3);
        }

        [Test]
        public void CleanRestorePointsHybridLimitAny()
        {
            _cleaner = new TestCleaner(new HybridLimit(
                new DateLimit(DateTime.Now.AddSeconds(4)),
                new AmountLimit(2), HybridType.Any));
            RestorePoint firstRestorePoint =
                _backupJob.AddNewRestorePoint(new SplitStorage(_files, _archiver),
                    @"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra\res", _repository);
            System.Threading.Thread.Sleep(5000);
            RestorePoint secondRestorePoint =
                _backupJob.AddNewRestorePoint(new SplitStorage(_files, _archiver),
                    @"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra\res", _repository);
            RestorePoint thirdRestorePoint =
                _backupJob.AddNewRestorePoint(new SplitStorage(_files, _archiver),
                    @"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra\res", _repository);
            RestorePoint fourthRestorePoint =
                _backupJob.AddNewRestorePoint(new SplitStorage(_files, _archiver),
                    @"C:\Users\BaHo\Documents\GitHub\t1mtg\BackupsExtra\res", _repository);
            _cleaner.Clean(_backupJob.GetRestorePoints());
            Assert.AreEqual(_backupJob.GetRestorePoints().Count, 2);
        }
    }
}