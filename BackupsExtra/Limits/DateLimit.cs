using System;
using System.Collections.Generic;
using System.Linq;

namespace BackupsExtra.Limits
{
    public class DateLimit : ILimit
    {
        public DateLimit(DateTime dateTime)
        {
            DateTime = dateTime;
        }

        public DateTime DateTime { get; }

        public IEnumerable<RestorePoint> GetRestorePointsToRemove(List<RestorePoint> restorePoints)
        {
            return restorePoints.Where(restorePoint => restorePoint.CreationTime < DateTime).ToList();
        }
    }
}