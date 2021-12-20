using System;
using System.Collections.Generic;
using System.Linq;

namespace BackupsExtra.Limits
{
    public enum HybridType
    {
        All,
        Any,
    }

    public class HybridLimit : Limit
    {
        public HybridLimit(DateLimit dateLimit, AmountLimit amountLimit, HybridType hybridType)
        {
            DateLimit = dateLimit;
            AmountLimit = amountLimit;
            HybridType = hybridType;
        }

        public DateLimit DateLimit { get; }
        public AmountLimit AmountLimit { get; }
        public HybridType HybridType { get; }

        public override IEnumerable<RestorePoint> GetRestorePointsToRemove()
        {
            switch (HybridType)
            {
                case HybridType.All:
                    return DateLimit.GetRestorePointsToRemove().Intersect(AmountLimit.GetRestorePointsToRemove());
                case HybridType.Any:
                    return DateLimit.GetRestorePointsToRemove().Union(AmountLimit.GetRestorePointsToRemove());
                default:
                    return null;
            }
        }
    }
}