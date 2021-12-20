using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BackupsExtra
{
    public class RestorePoint
    {
        public RestorePoint(JobObject jobJobObject, List<string> storagesPaths, DateTime dateTime)
        {
            Storages = new List<string>();
            CreationTime = dateTime;
            JobObject = jobJobObject;
            Storages.AddRange(storagesPaths);
        }

        public RestorePoint() { }

        public JobObject JobObject { get; }
        public DateTime CreationTime { get; set; }
        public List<string> Storages { get; }
        public RestorePointCreationSettings Settings { get; set; }

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