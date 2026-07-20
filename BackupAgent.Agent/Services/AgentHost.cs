using System;
using System.IO;
using System.Threading;
using BackupAgent.Core.Logging;

namespace BackupAgent.Agent.Services
{
    public class AgentHost
    {
        private FileStream? _lock;
        private CancellationTokenSource? _cts;

        public void Run()
        {
            if (!AcquireLock())
            {
                BackupLogger.Write(
                    "Another BackupAgent instance is already running");

                return;
            }

            BackupLogger.Write("BackupAgent starting");

            _cts = new CancellationTokenSource();

            BackupLogger.Write("BackupAgent running");

            RunLoop(_cts.Token);
        }

        private void RunLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    Thread.Sleep(1000);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        private bool AcquireLock()
        {
            // Per-user single-instance guard: the exclusive file handle is
            // held for the whole process lifetime. If the process dies (even
            // on a crash or kill), the OS releases the handle automatically,
            // so there is no stale lock to clean up. Works identically on
            // Windows, Linux and macOS.
            var directory = Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData);

            var path = Path.Combine(directory, "BackupAgent.lock");

            try
            {
                _lock = new FileStream(
                    path,
                    FileMode.OpenOrCreate,
                    FileAccess.ReadWrite,
                    FileShare.None,
                    bufferSize: 1,
                    FileOptions.DeleteOnClose);

                BackupLogger.Write(
                    $"Lock acquired: {path}");

                return true;
            }
            catch (IOException)
            {
                BackupLogger.Write(
                    $"Lock already held: {path}");

                return false;
            }
        }

        public void Stop()
        {
            BackupLogger.Write("BackupAgent stopping");

            // Signalling the token is safe from any thread, unlike
            // Mutex.ReleaseMutex which is bound to the acquiring thread.
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;

            if (_lock != null)
            {
                _lock.Dispose();
                _lock = null;
            }
        }
    }
}
