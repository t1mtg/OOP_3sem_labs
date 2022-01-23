using System;
using System.Collections.Generic;

namespace Backups
{
    public class RestorePoint
    {
        private List<string> _storages;

        public RestorePoint(JobObject @jobObject, IEnumerable<string> paths)
        {
            _storages = new List<string>();
            CreationTime = DateTime.Now;
            Storages = _storages.AsReadOnly();
            JobObject = @jobObject;
            _storages.AddRange(paths);
        }

        public DateTime CreationTime { get; }
        public IReadOnlyList<string> Storages { get; }

        private JobObject JobObject { get; }
    }
}