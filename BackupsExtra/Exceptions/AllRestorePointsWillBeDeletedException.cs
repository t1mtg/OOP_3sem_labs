using System;

namespace BackupsExtra.Exceptions
{
    public class AllRestorePointsWillBeDeletedException : Exception
    {
        public AllRestorePointsWillBeDeletedException()
            : base("All restore points will be deleted.")
        {
        }
    }
}