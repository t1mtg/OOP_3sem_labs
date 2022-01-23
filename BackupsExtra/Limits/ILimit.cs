using System.Collections.Generic;

namespace BackupsExtra.Limits
{
    public interface ILimit
    {
        public IEnumerable<RestorePoint> GetRestorePointsToRemove(List<RestorePoint> restorePoints);
    }
}