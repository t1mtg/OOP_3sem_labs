using System;
using System.Collections.Generic;
using System.Linq;
using BackupsExtra.Exceptions;

namespace BackupsExtra.Limits
{
    public class AmountLimit : Limit
    {
        public AmountLimit(List<RestorePoint> restorePoints, int amount)
        {
            Amount = amount;
            RestorePoints = restorePoints;
        }

        public int Amount { get; }
        public List<RestorePoint> RestorePoints { get; }

        public override IEnumerable<RestorePoint> GetRestorePointsToRemove()
        {
            if (Amount >= RestorePoints.Count)
            {
                throw new AllRestorePointsWillBeDeletedException();
            }

            return RestorePoints.Take(RestorePoints.Count - Amount).ToList();
        }
    }
}