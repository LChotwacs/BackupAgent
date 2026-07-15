using System;
using System.Threading;
using BackupAgent.Core.Logging;

namespace BackupAgent.Agent.Services
{
    public class AgentHost
    {
        private bool _running;

        public void Run()
        {
            BackupLogger.Write("BackupAgent starting");

            _running = true;

            BackupLogger.Write("BackupAgent running");

            RunLoop();
        }

        private void RunLoop()
        {
            while (_running)
            {
                Thread.Sleep(1000);
            }
        }

        public void Stop()
        {
            BackupLogger.Write("BackupAgent stopping");

            _running = false;
        }
    }
}