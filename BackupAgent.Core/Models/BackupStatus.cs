namespace BackupAgent.Core.Models
{
    public class BackupStatus
    {
        public bool Running { get; set; }
        
        public bool AgentAlive { get; set; }

        public string LastHeartbeat { get; set; }

        public string LastBackupTime { get; set; }

        public string LastError { get; set; }

        public int CopiedFiles { get; set; }

        public string CurrentSessionFolder { get; set; }
    }
}