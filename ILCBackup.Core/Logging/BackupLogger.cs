using System;
using System.IO;

namespace ILCBackup.Core.Logging
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
                if (string.IsNullOrEmpty(_logFilePath))
                    return;

                string logLine =
                    $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";

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