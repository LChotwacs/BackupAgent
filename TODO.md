# BackupAgent — TODO / Roadmap

A cross-platform (Windows / Linux / macOS) backup tool in C# / .NET 10.
GUI (Avalonia) and the background Agent communicate only through JSON files
in the per-user app-data directory (`config.json`, `command.json`, `status.json`).

## Architecture at a glance

```
        GUI                     JSON files (IPC)            Agent
   writes config      ── config.json  ─────────────►   ConfigManager.Load()
   sends commands     ── command.json (start/stop) ─►   CommandManager.Read()
   reads status       ◄─ status.json ────────────────   StatusManager.Save()
```

## Done

- [x] Cross-platform single-instance guard via an exclusive lock file
      (`AgentHost`, replaces the Windows-only named Mutex)
- [x] `.gitignore` + repository cleanup (untracked build artifacts)

## Next up

### 1. Make the Agent "alive" (highest value — do this first)
- [ ] `AgentHost.RunLoop()`: poll `CommandManager.Read()` each cycle
- [ ] On `"start"` → load `ConfigManager.Load()` and call `BackupService.Start(config)`
- [ ] On `"stop"` → `BackupService.Stop()`
- [ ] Write a periodic heartbeat to `status.json`
      (`AgentAlive`, `LastHeartbeat` — currently nothing sets these)
- [ ] Manual test: drop a `command.json` by hand and watch a backup run

### 2. Build the GUI (control surface on top of the working Agent)
- [ ] Config screen: edit + save `config.json` (source path, backup path, interval)
- [ ] Start / Stop buttons → `CommandManager.Send(...)`
- [ ] Live status display: poll `status.json` (running, last backup, copied files, errors)
- [ ] Replace the "Welcome to Avalonia!" template in `MainViewModel`

## Backlog / cleanup

- [ ] Cross-platform path fix: `BackupConfig` uses a hardcoded `"\\EPLAN_Backups"`
      backslash — switch to `Path.Combine(...)`
- [ ] Unify data directory: managers use `ApplicationData`, the lock file uses
      `LocalApplicationData` — decide on one
- [ ] Debounce the `FileSystemWatcher` (currently every change triggers a full backup)
- [ ] Address the nullable-reference warnings across `BackupAgent.Core`
- [ ] Translate the German log/UI strings to English (open-source consistency)
