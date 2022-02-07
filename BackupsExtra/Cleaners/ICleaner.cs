using System.Collections.Generic;

namespace BackupsExtra.Cleaners
{
    public interface ICleaner
    {
        public void Clean(List<RestorePoint> restorePoints, bool isNotMerge = true);
    }
}