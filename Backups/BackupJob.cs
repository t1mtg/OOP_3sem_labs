using System.Collections.Generic;
using System.Linq;
using Backups.Interfaces;

namespace Backups
{
    public class BackupJob
    {
        private List<RestorePoint> _restorePoints;

        public BackupJob(JobObject jobObject)
        {
            _restorePoints = new List<RestorePoint>();
            JobObject = jobObject;
            NumberOfRestorePoints = 0;
        }

        public uint NumberOfRestorePoints { get; set; }
        public JobObject JobObject { get; }

        public void AddNewRestorePoint(Algorithm algorithm, FileSystemConfig config, string outputDirPath, Irepository repository)
        {
            List<string> newStorages = repository.Save(algorithm, config, NumberOfRestorePoints, outputDirPath);
            var restorePoint = new RestorePoint(JobObject, newStorages);
            _restorePoints.Add(restorePoint);
            NumberOfRestorePoints++;
        }

        public List<string> GetListOfStorages()
        {
            return _restorePoints.SelectMany(point => point.Storages).ToList();
        }

        public List<RestorePoint> GetListOfRestorePoints()
        {
            return _restorePoints;
        }

        public void RemoveObject(string obj)
        {
            JobObject.RemoveJobObject(obj);
        }
    }
}