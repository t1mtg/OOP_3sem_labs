using System.Collections.Generic;

namespace Backups
{
    public class JobObject
    {
        private readonly List<string> _jobObjectFiles;

        public JobObject(List<string> jobObjectFiles)
        {
            _jobObjectFiles = jobObjectFiles;
            JobObjectFiles = jobObjectFiles.AsReadOnly();
        }

        public IReadOnlyList<string> JobObjectFiles { get; }

        public void RemoveJobObject(string jobObject)
        {
            _jobObjectFiles.Remove(jobObject);
        }
    }
}