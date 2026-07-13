using System.Timers;
using ILCBackup.Core.Models;
using ILCBackup.Core.Logging;
using Timer = System.Timers.Timer;

namespace ILCBackup.Core.Services
{
    public class BackupService
    {
        private Timer _timer;
        private FileSystemWatcher _watcher;

        private BackupConfig _config;

        private string _sessionFolder;

        private BackupStatus _status;

        public bool IsRunning { get; private set; }

        public void Start(BackupConfig config)
        {
            if (IsRunning)
                return;

            _config = config;

            _status = new BackupStatus();

            _status.Running = true;

            CreateSessionFolder();

            _status.CurrentSessionFolder = _sessionFolder;

            BackupLogger.Initialize(_sessionFolder);

            BackupLogger.Write("Backup-Service gestartet");

            StatusManager.Save(_status);

            _timer = new Timer(config.IntervalMinutes * 60 * 1000);

            _timer.Elapsed += OnTimerElapsed;

            _timer.Start();

            StartWatcher();

            IsRunning = true;

            BackupNow();
        }

        public void Stop()
        {
            if (!IsRunning)
                return;

            _timer?.Stop();

            _timer?.Dispose();

            _watcher?.Dispose();

            BackupLogger.Write("Backup-Service gestoppt");

            _status.Running = false;

            StatusManager.Save(_status);

            IsRunning = false;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            BackupNow();
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                BackupLogger.Write(
                    "Dateiänderung erkannt: "
                    + e.FullPath);

                BackupNow();
            }
            catch (Exception ex)
            {
                BackupLogger.Write(
                    "Watcher-Fehler: "
                    + ex.Message);
            }
        }

        private void CreateSessionFolder()
        {
            string timestamp =
                DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

            _sessionFolder =
                Path.Combine(
                    _config.BackupPath,
                    timestamp + "_EPLAN-Session");

            Directory.CreateDirectory(_sessionFolder);
        }

        private void StartWatcher()
        {
            _watcher = new FileSystemWatcher();

            _watcher.Path = _config.SourcePath;

            _watcher.IncludeSubdirectories = true;

            _watcher.EnableRaisingEvents = true;

            _watcher.Created += OnFileChanged;

            _watcher.Changed += OnFileChanged;

            _watcher.Renamed += OnFileChanged;

            BackupLogger.Write(
                "Dateiüberwachung gestartet");
        }

        public void BackupNow()
        {
            try
            {
                int copiedFiles = 0;

                if (!Directory.Exists(_config.SourcePath))
                {
                    BackupLogger.Write(
                        "Quellordner nicht gefunden: "
                        + _config.SourcePath);

                    return;
                }

                string[] files =
                    Directory.GetFiles(
                        _config.SourcePath,
                        "*",
                        SearchOption.AllDirectories);

                foreach (string file in files)
                {
                    string relativePath =
                        file.Substring(
                            _config.SourcePath.Length + 1);

                    string destination =
                        Path.Combine(
                            _sessionFolder,
                            relativePath);

                    string destinationDirectory =
                        Path.GetDirectoryName(destination);

                    if (!Directory.Exists(destinationDirectory))
                    {
                        Directory.CreateDirectory(
                            destinationDirectory);
                    }

                    bool shouldCopy = true;

                    if (File.Exists(destination))
                    {
                        FileInfo sourceInfo =
                            new FileInfo(file);

                        FileInfo destinationInfo =
                            new FileInfo(destination);

                        bool sameSize =
                            sourceInfo.Length ==
                            destinationInfo.Length;

                        bool sameWriteTime =
                            sourceInfo.LastWriteTime ==
                            destinationInfo.LastWriteTime;

                        if (sameSize && sameWriteTime)
                        {
                            shouldCopy = false;
                        }
                    }

                    if (shouldCopy)
                    {
                        File.Copy(file, destination, true);
                        copiedFiles++;

                        File.SetLastWriteTime(
                            destination,
                            File.GetLastWriteTime(file));

                        BackupLogger.Write(
                            "Gesichert: " + relativePath);
                    }
                    else
                    {
                        BackupLogger.Write(
                            "Übersprungen: " + relativePath);
                    }
                }

                BackupLogger.Write(
                    "Backup erfolgreich abgeschlossen");

                _status.LastBackupTime =
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                _status.CopiedFiles =
                    copiedFiles;

                _status.LastError = "";

                StatusManager.Save(_status);
            }
            catch (Exception ex)
            {
                BackupLogger.Write(
                    "FEHLER: " + ex.Message);

                _status.LastError = ex.Message;

                StatusManager.Save(_status);
            }
        }
    }
}