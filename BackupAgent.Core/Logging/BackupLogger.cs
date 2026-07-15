using System;
using System.IO;

namespace BackupAgent.Core.Logging
{
    public static class BackupLogger
    {
        private static string _logFilePath;

        public static void Initialize(string sessionFolder)
        {
            _logFilePath =
                Path.Combine(sessionFolder, "backup-log.txt");

            Write("Logger initialisiert");
        }

        public static void Write(string message)
        {
            try
            {
                string logLine =
                    $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
                
                Console.WriteLine(logLine);
                
                if (string.IsNullOrEmpty(_logFilePath))
                    return;

                File.AppendAllText(
                    _logFilePath,
                    logLine + Environment.NewLine);
            }
            catch
            {
                // Logger darf niemals abstürzen
            }
        }
    }
}