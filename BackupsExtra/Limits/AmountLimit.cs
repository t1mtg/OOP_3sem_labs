using System;
using System.Collections.Generic;
using System.Linq;
using BackupsExtra.Exceptions;

namespace BackupsExtra.Limits
{
    public class AmountLimit : ILimit
    {
        public AmountLimit(int amount)
        {
            Amount = amount;
        }

        public int Amount { get; }

        public IEnumerable<RestorePoint> GetRestorePointsToRemove(List<RestorePoint> restorePoints)
        {
            if (Amount >= restorePoints.Count)
            {
                throw new AllRestorePointsWillBeDeletedException();
            }

            return restorePoints.Take(restorePoints.Count - Amount).ToList();
        }
    }
}