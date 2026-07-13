using System;

namespace BackupAgent.Core.Models
{
    public class BackupConfig
    {
        public string SourcePath { get; set; }

        public string BackupPath { get; set; }

        public int IntervalMinutes { get; set; }

        public bool AutoStart { get; set; }

        public bool EnableLogging { get; set; }

        public BackupConfig()
        {
            SourcePath = "";

            BackupPath =
                Environment.GetFolderPath(
                    Environment.SpecialFolder.MyDocuments)
                + "\\EPLAN_Backups";

            IntervalMinutes = 60;

            AutoStart = false;

            EnableLogging = true;
        }
    }
}