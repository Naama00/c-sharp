using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace tools
{
    public static class LogManager
    {
        private const string LogFolderName = "Log";

        public static string GetLogDirectoryPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LogFolderName);
        }
        public static string GetCurrentLogFilePath()
        {
            string directoryPath = GetLogDirectoryPath();

            string fileName = $"Log_{DateTime.Now:yyyy-MM-dd}.txt";

            return Path.Combine(directoryPath, fileName);
        }

        public static void Log(string projectName, string functionName, string message)
        {
            string logFilePath = GetCurrentLogFilePath();

            Directory.CreateDirectory(GetLogDirectoryPath());

            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - [{projectName}] - [{functionName}] - {message}";

            File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
        }

        public static void deleteOldLogs()
        {
            int daysToKeep = 60;
            string logDirectory = GetLogDirectoryPath();

            if (Directory.Exists(logDirectory))
            {
                var logFiles = Directory.GetFiles(logDirectory, "Log_*.txt");

                foreach (var logFile in logFiles)
                {
                    DateTime creationTime = File.GetCreationTime(logFile);
                    if ((DateTime.Now - creationTime).TotalDays > daysToKeep)
                    {
                        File.Delete(logFile);
                    }
                }
            }
        }



    }
}