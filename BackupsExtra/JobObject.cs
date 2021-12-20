using System.Collections.Generic;

namespace BackupsExtra
{
    public class JobObject
    {
        public JobObject(List<string> jobsFiles)
        {
            Files = jobsFiles.AsReadOnly();
        }

        public JobObject()
        {
        }

        public IReadOnlyList<string> Files { get; }
    }
}