using System;
using System.IO;
using System.Text.Json;
using ILCBackup.Core.Models;

namespace ILCBackup.Core.Services
{
    public static class CommandManager
    {
        private static readonly string CommandDirectory =
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData),
                "ILCBackup");

        private static readonly string CommandFile =
            Path.Combine(
                CommandDirectory,
                "command.json");

        public static void Send(string command)
        {
            try
            {
                if (!Directory.Exists(CommandDirectory))
                {
                    Directory.CreateDirectory(
                        CommandDirectory);
                }

                BackupCommand cmd =
                    new BackupCommand
                    {
                        Command = command
                    };

                string json =
                    JsonSerializer.Serialize(
                        cmd,
                        new JsonSerializerOptions
                        {
                            WriteIndented = true
                        });

                File.WriteAllText(
                    CommandFile,
                    json);
            }
            catch
            {
            }
        }

        public static BackupCommand Read()
        {
            try
            {
                if (!File.Exists(CommandFile))
                {
                    return null;
                }

                string json =
                    File.ReadAllText(CommandFile);

                File.Delete(CommandFile);

                return JsonSerializer.Deserialize<BackupCommand>(json);
            }
            catch
            {
                return null;
            }
        }
    }
}