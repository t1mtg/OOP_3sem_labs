using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json.Serialization;
using BackupsExtra.Exceptions;
using BackupsExtra.Limits;
using Newtonsoft.Json;

namespace BackupsExtra
{
    public class BackupJob
    {
        public BackupJob(JobObject job, BackupLogger logger)
        {
            RestorePoints = new List<RestorePoint>();
            JobObject = job;
            NumberOfRestorePoint = 0;
            Logger = logger;
        }

        public BackupJob()
        {
        }

        public JobObject JobObject { get; set; }
        public List<RestorePoint> RestorePoints { get; set; }
        public int NumberOfRestorePoint { get; set; }
        public BackupLogger Logger { get; set; }

        public static void Save(string path, BackupJob job)
        {
            string json = JsonConvert.SerializeObject(job);
            using var streamWriter = new StreamWriter(path);
            streamWriter.WriteLine(json);
        }

        public static BackupJob Get(string path)
        {
            using var streamReader = new StreamReader(path);
            string json = streamReader.ReadToEnd();
            return JsonConvert.DeserializeObject<BackupJob>(json);
        }

        public List<RestorePoint> GetRestorePoints()
        {
            return RestorePoints;
        }

        public RestorePoint AddNewRestorePoint(IAlgorithm algorithm, string outputDirPath, IRepository repository)
        {
            IEnumerable<string> newStorages = repository.Save(algorithm, NumberOfRestorePoint, outputDirPath);
            var restorePoint = new RestorePoint(JobObject, newStorages);
            restorePoint.Algorithm = algorithm;
            RestorePoints.Add(restorePoint);
            NumberOfRestorePoint++;
            return restorePoint;
        }

        public void RestoreFilesFromBackup(RestorePoint restorePoint, RestoreLocation restoreLocation, string restorePath = default)
        {
            switch (restoreLocation)
            {
                case RestoreLocation.Original:
                    restorePath = restorePoint.JobObject.Files.First();
                    restorePath = restorePath[..restorePath.LastIndexOf(Path.DirectorySeparatorChar)];
                    break;
                case RestoreLocation.Different:
                    if (restorePath == default)
                    {
                        throw new IncorrectPathException();
                    }

                    break;
            }

            foreach (string path in restorePoint.Storages)
            {
                ZipFile.ExtractToDirectory(path, restorePath, true);
            }
        }
    }
}