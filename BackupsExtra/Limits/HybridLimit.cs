﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace BackupsExtra.Limits
{
    public enum HybridType
    {
        All,
        Any,
    }

    public class HybridLimit : ILimit
    {
        public HybridLimit(ILimit firstLimit, ILimit secondLimit, HybridType hybridType)
        {
            FirstLimit = firstLimit;
            SecondLimit = secondLimit;
            HybridType = hybridType;
        }

        public ILimit FirstLimit { get; }
        public ILimit SecondLimit { get; }
        public HybridType HybridType { get; }

        public IEnumerable<RestorePoint> GetRestorePointsToRemove()
        {
            switch (HybridType)
            {
                case HybridType.All:
                    return FirstLimit.GetRestorePointsToRemove().Intersect(SecondLimit.GetRestorePointsToRemove());
                case HybridType.Any:
                    return FirstLimit.GetRestorePointsToRemove().Union(SecondLimit.GetRestorePointsToRemove());
                default:
                    return null;
            }
        }
    }
}