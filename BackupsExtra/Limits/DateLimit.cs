using System;
using System.Collections.Generic;
using System.Linq;

namespace BackupsExtra.Limits
{
    public class DateLimit : ILimit
    {
        public DateLimit(List<RestorePoint> restorePoints, DateTime dateTime)
        {
            DateTime = dateTime;
            RestorePoints = restorePoints;
        }

        public DateTime DateTime { get; }
        public List<RestorePoint> RestorePoints { get; }

        public IEnumerable<RestorePoint> GetRestorePointsToRemove()
        {
            return RestorePoints.Where(restorePoint => restorePoint.CreationTime < DateTime).ToList();
        }
    }
}