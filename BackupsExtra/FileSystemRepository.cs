using System;
using System.Collections.Generic;

namespace BackupsExtra
{
    public class FileSystemRepository
    {
        private readonly List<string> _listOfStorages = new List<string>();
        private List<string> _listOfJobObjects;

        public FileSystemRepository(List<string> listOfObjects)
        {
            _listOfJobObjects = listOfObjects;
        }

        public List<string> GetStorages()
        {
            return _listOfStorages;
        }

        public List<string> Save(RestorePointCreationSettings settings, int numberOfRestorePoint, string pathToSave)
        {
            switch (settings.StorageTypeConfig)
            {
                case StorageTypeConfig.SingleStorage:
                    var fileSystemSingle = new FileSystemSingle(_listOfJobObjects);
                    fileSystemSingle.Archivate(settings.FileSystemConfig, numberOfRestorePoint, pathToSave);
                    _listOfStorages.AddRange(fileSystemSingle.GetStorages());
                    return fileSystemSingle.GetStorages();
                case StorageTypeConfig.SplitStorage:
                    var fileSystemSplit = new FileSystemSplit(_listOfJobObjects);
                    fileSystemSplit.Archivate(settings.FileSystemConfig, numberOfRestorePoint, pathToSave);
                    _listOfStorages.AddRange(fileSystemSplit.GetStorages());
                    return fileSystemSplit.GetStorages();
                default:
                    return null;
            }
        }
    }
}