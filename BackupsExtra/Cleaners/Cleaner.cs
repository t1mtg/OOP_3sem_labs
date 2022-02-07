using System.Collections.Generic;
using System.IO;
using System.Linq;
using BackupsExtra.Exceptions;
using BackupsExtra.Limits;

namespace BackupsExtra.Cleaners
{
    public class Cleaner : ICleaner
    {
        public Cleaner(ILimit limit)
        {
            Limit = limit;
        }

        public Cleaner()
        {
        }

        public ILimit Limit { get; set; }

        public void Clean(List<RestorePoint> restorePoints, bool isNotMerge = true)
        {
            IEnumerable<RestorePoint> restorePointsToRemove = Limit.GetRestorePointsToRemove(restorePoints);
            if (restorePointsToRemove.Count() >= restorePoints.Count && isNotMerge)
            {
                throw new AllRestorePointsWillBeDeletedException();
            }

            IAlgorithm algorithm = restorePointsToRemove.First().Algorithm;
            foreach (RestorePoint restorePoint in restorePointsToRemove)
            {
                algorithm.DeleteStorages(restorePoint.Storages);
                restorePoints.Remove(restorePoint);
            }
        }

        public void Merge(List<RestorePoint> restorePoints)
        {
            IEnumerable<RestorePoint> restorePointsToRemove = Limit.GetRestorePointsToRemove(restorePoints);
            if (restorePointsToRemove.Count() >= restorePoints.Count)
            {
                throw new AllRestorePointsWillBeDeletedException();
            }

            IEnumerable<RestorePoint> restorePointsToSave = restorePoints.Except(Limit.GetRestorePointsToRemove(restorePoints));
            RestorePoint restorePointToMergeWith = restorePointsToSave.First();
            foreach (RestorePoint restorePoint in restorePointsToRemove)
            {
                MergeRestorePoints(restorePointToMergeWith, restorePoint);
            }
        }

        public void MergeRestorePoints(RestorePoint newRestorePoint, RestorePoint oldRestorePoint)
        {
            if (newRestorePoint.Algorithm is SingleStorage || oldRestorePoint.Algorithm is SingleStorage)
            {
                DeleteOldRestorePoint(oldRestorePoint);
                return;
            }

            var filesToAdd = oldRestorePoint.Storages
                .Where(file => newRestorePoint.Storages.FirstOrDefault(s => GetFileName(s)
                    .Equals(GetFileName(file))) == null).ToList();
            foreach (string file in filesToAdd)
            {
                newRestorePoint.AddFile(file);
                File.Move(
                    file,
                    GetDestinationFileName(newRestorePoint, file),
                    true);
            }

            DeleteOldRestorePoint(oldRestorePoint);
        }

        private string GetFileName(string file)
        {
            return file.Substring(
                file.LastIndexOf(Path.DirectorySeparatorChar),
                file.LastIndexOf('_') - file.LastIndexOf(Path.DirectorySeparatorChar));
        }

        private string GetDestinationFileName(RestorePoint restorePoint, string file)
        {
            return restorePoint.Storages[0][..file.LastIndexOf(Path.DirectorySeparatorChar)] +
                   file[file.LastIndexOf(Path.DirectorySeparatorChar) ..];
        }

        private void DeleteOldRestorePoint(RestorePoint oldRestorePoint)
        {
            Clean(
                new List<RestorePoint>()
                {
                    oldRestorePoint,
                }, isNotMerge: false);
        }
    }
}