using System.Collections.Generic;
using System.Linq;
using BackupsExtra.Exceptions;
using BackupsExtra.Limits;

namespace BackupsExtra.Cleaners
{
    public class TestCleaner : ICleaner
    {
        public TestCleaner(ILimit limit)
        {
            Limit = limit;
        }

        public ILimit Limit { get; set; }

        public void Clean(List<RestorePoint> restorePoints, bool isNotMerge = true)
        {
            IEnumerable<RestorePoint> restorePointsToRemove = Limit.GetRestorePointsToRemove(restorePoints);
            if (restorePointsToRemove.Count() >= restorePoints.Count && isNotMerge)
            {
                throw new AllRestorePointsWillBeDeletedException();
            }

            foreach (RestorePoint restorePoint in restorePointsToRemove)
            {
                restorePoints.Remove(restorePoint);
            }
        }
    }
}