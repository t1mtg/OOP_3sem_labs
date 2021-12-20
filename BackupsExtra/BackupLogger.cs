using System;
using System.IO;

namespace BackupsExtra
{
    public enum LogDate
    {
        DatePrefix,
        NoPrefix,
    }

    public enum LogDestination
    {
        File,
        Console,
    }

    public class BackupLogger
    {
        private LogDestination _logDestination;
        private LogDate _logDate;
        private string _file;

        public BackupLogger(LogDestination logDestination, LogDate logDate, string file = default)
        {
            _logDate = logDate;
            _logDestination = logDestination;
            _file = file;
        }

        public BackupLogger()
        {
        }

        public void Log(string message)
        {
            if (_logDate == LogDate.DatePrefix)
            {
                message = DateTime.Now + "_" + message;
            }

            switch (_logDestination)
            {
                case LogDestination.Console:
                    Console.WriteLine(message);
                    return;
                case LogDestination.File:
                    var sw = new StreamWriter(_file);
                    sw.WriteLine(message);
                    sw.Close();
                    return;
            }
        }
    }
}