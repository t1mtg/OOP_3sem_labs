using System.Collections.Generic;

namespace BackupsExtra.Limits
{
    public interface ILimit
    {
        public abstract IEnumerable<RestorePoint> GetRestorePointsToRemove();
    }
}