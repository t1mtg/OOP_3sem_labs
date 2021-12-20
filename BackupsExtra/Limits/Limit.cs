using System.Collections.Generic;

namespace BackupsExtra.Limits
{
    public abstract class Limit
    {
        public abstract IEnumerable<RestorePoint> GetRestorePointsToRemove();
    }
}