using System;

namespace BackupsExtra
{
    public enum FileSystemConfig
    {
        Folder,
        Tests,
    }

    public enum StorageTypeConfig
    {
        Split,
        Single,
    }

    public class RestorePointCreationSettings
    {
        public RestorePointCreationSettings(DateTime dateTime, StorageTypeConfig storageTypeConfig, FileSystemConfig fileSystemConfig)
        {
            DateTime = dateTime;
            StorageTypeConfig = storageTypeConfig;
            FileSystemConfig = fileSystemConfig;
        }

        public DateTime DateTime { get; }
        public StorageTypeConfig StorageTypeConfig { get; }
        public FileSystemConfig FileSystemConfig { get; }
    }
}