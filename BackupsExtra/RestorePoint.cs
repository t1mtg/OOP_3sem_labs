using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BackupsExtra
{
    public class RestorePoint
    {
        public RestorePoint(JobObject @jobObject, IEnumerable<string> paths)
        {
            Storages = new List<string>();
            CreationTime = DateTime.Now;
            JobObject = @jobObject;
            Storages.AddRange(paths);
        }

        public DateTime CreationTime { get; }
        public IAlgorithm Algorithm { get; set; }
        public List<string> Storages { get; }

        public JobObject JobObject { get; }

        public void RemoveLastFile()
        {
            if (Storages.Count == 1) return;
            File.Delete(Storages.Last());
            Storages.Remove(Storages.Last());
        }

        public void AddFile(string file)
        {
            Storages.Add(file);
        }
    }
}