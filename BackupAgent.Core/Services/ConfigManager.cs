using System;
using System.IO;
using System.Text.Json;
using BackupAgent.Core.Models;

namespace BackupAgent.Core.Services
{
    public class ConfigManager
    {
        private static readonly string ConfigDirectory =
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData),
                "BackupAgent");

        private static readonly string ConfigFile =
            Path.Combine(ConfigDirectory, "config.json");

        public static BackupConfig Load()
        {
            try
            {
                if (!Directory.Exists(ConfigDirectory))
                {
                    Directory.CreateDirectory(ConfigDirectory);
                }

                if (!File.Exists(ConfigFile))
                {
                    BackupConfig defaultConfig = new BackupConfig();

                    Save(defaultConfig);

                    return defaultConfig;
                }

                string json = File.ReadAllText(ConfigFile);

                return JsonSerializer.Deserialize<BackupConfig>(json);
            }
            catch
            {
                return new BackupConfig();
            }
        }

        public static void Save(BackupConfig config)
        {
            if (!Directory.Exists(ConfigDirectory))
            {
                Directory.CreateDirectory(ConfigDirectory);
            }

            string json =
                JsonSerializer.Serialize(
                    config,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

            File.WriteAllText(ConfigFile, json);
        }
    }
}