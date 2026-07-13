using System;
using System.IO;
using System.Text.Json;
using ILCBackup.Core.Models;

namespace ILCBackup.Core.Services
{
    public static class StatusManager
    {
        private static readonly string StatusDirectory =
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData),
                "ILCBackup");

        private static readonly string StatusFile =
            Path.Combine(StatusDirectory, "status.json");

        public static void Save(BackupStatus status)
        {
            try
            {
                if (!Directory.Exists(StatusDirectory))
                {
                    Directory.CreateDirectory(StatusDirectory);
                }

                string json =
                    JsonSerializer.Serialize(
                        status,
                        new JsonSerializerOptions
                        {
                            WriteIndented = true
                        });

                File.WriteAllText(StatusFile, json);
            }
            catch
            {
                // Status darf niemals crashen
            }
        }

        public static BackupStatus Load()
        {
            try
            {
                if (!File.Exists(StatusFile))
                {
                    return new BackupStatus();
                }

                string json =
                    File.ReadAllText(StatusFile);

                return JsonSerializer.Deserialize<BackupStatus>(json);
            }
            catch
            {
                return new BackupStatus();
            }
        }
    }
}