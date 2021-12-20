using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using BackupsExtra.Exceptions;

namespace BackupsExtra.Limits
{
    public class Cleaner
    {
        public Cleaner(Limit limit)
        {
            Limit = limit;
        }

        public Cleaner()
        {
        }

        public static Limit Limit { get; set; }

        public static void Clean(List<RestorePoint> restorePoints, FileSystemConfig fileSystemConfig = FileSystemConfig.Folder, bool isNotMerge = true)
        {
            IEnumerable<RestorePoint> restorePointsToRemove = Limit.GetRestorePointsToRemove();
            if (restorePointsToRemove.Count() >= restorePoints.Count && isNotMerge)
            {
                throw new AllRestorePointsWillBeDeletedException();
            }

            switch (fileSystemConfig)
            {
                case FileSystemConfig.Folder:
                    foreach (RestorePoint restorePoint in restorePointsToRemove)
                    {
                        switch (restorePoint.Settings.StorageTypeConfig)
                        {
                            case StorageTypeConfig.Single:
                                foreach (string file in restorePoint.Storages)
                                {
                                    Directory.Delete(file[..file.LastIndexOf(Path.DirectorySeparatorChar)], true);
                                }

                                break;
                            case StorageTypeConfig.Split:
                                string firstFile = restorePoint.Storages[0];
                                Directory.Delete(firstFile[..firstFile.LastIndexOf(Path.DirectorySeparatorChar)], true);
                                break;
                        }

                        restorePoints.Remove(restorePoint);
                    }

                    return;
                case FileSystemConfig.Tests:
                    foreach (RestorePoint restorePoint in restorePointsToRemove)
                    {
                        restorePoints.Remove(restorePoint);
                    }

                    return;
            }
        }

        public static void Merge(List<RestorePoint> restorePoints)
        {
            IEnumerable<RestorePoint> restorePointsToRemove = Limit.GetRestorePointsToRemove();
            if (restorePointsToRemove.Count() >= restorePoints.Count)
            {
                throw new AllRestorePointsWillBeDeletedException();
            }

            IEnumerable<RestorePoint> restorePointsToSave = restorePoints.Except(Limit.GetRestorePointsToRemove());
            RestorePoint restorePointToMergeWith = restorePointsToSave.First();
            foreach (RestorePoint restorePoint in restorePointsToRemove)
            {
                MergeRestorePoints(restorePointToMergeWith, restorePoint);
            }
        }

        public static void MergeRestorePoints(RestorePoint newRestorePoint, RestorePoint oldRestorePoint)
        {
            if (newRestorePoint.Settings.StorageTypeConfig == StorageTypeConfig.Single ||
                oldRestorePoint.Settings.StorageTypeConfig == StorageTypeConfig.Single)
            {
                Clean(
                    new List<RestorePoint>()
                    {
                        oldRestorePoint,
                    }, isNotMerge: false);
                return;
            }

            var filesToAdd = oldRestorePoint.Storages
                .Where(file => newRestorePoint.Storages.FirstOrDefault(s => s
                    .Substring(
                        s.LastIndexOf(Path.DirectorySeparatorChar),
                        s.LastIndexOf('_') - s.LastIndexOf(Path.DirectorySeparatorChar))
                    .Equals(file.Substring(
                        file.LastIndexOf(Path.DirectorySeparatorChar),
                        file.LastIndexOf('_') - file.LastIndexOf(Path.DirectorySeparatorChar)))) == null).ToList();
            foreach (string file in filesToAdd)
            {
                string tmp = newRestorePoint.Storages[0][..file.LastIndexOf(Path.DirectorySeparatorChar)] +
                             file[file.LastIndexOf(Path.DirectorySeparatorChar) ..];
                newRestorePoint.AddFile(file);
                File.Move(
                    file,
                    newRestorePoint.Storages[0][..file.LastIndexOf(Path.DirectorySeparatorChar)] + file[file.LastIndexOf(Path.DirectorySeparatorChar) ..],
                    true);
            }

            Clean(
                new List<RestorePoint>()
                {
                    oldRestorePoint,
                }, isNotMerge: false);
        }
    }
}