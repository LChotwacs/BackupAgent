# ILCBackup

ILCBackup is a cross-platform backup automation tool written in C# and .NET.

The project provides an automated backup agent with a separated core architecture and a future cross-platform user interface.

## Architecture

The solution is separated into three main components:

### ILCBackup.Core
Contains the core functionality and business logic:

- Backup handling
- Configuration management
- Status management
- Command processing
- Logging

### ILCBackup.Agent
Background process responsible for:

- Running the backup workflow
- Processing commands
- Scheduling tasks
- Managing the application lifecycle

### ILCBackup.GUI
Cross-platform user interface based on Avalonia UI.

## Technologies

- C#
- .NET 10
- Avalonia UI
- System.Text.Json

## Project Status

Currently under active development.

The project is being migrated from an earlier Windows-only implementation to a modern cross-platform .NET architecture.

## Goals

- Cross-platform support (Linux / Windows)
- Clean separation between UI and business logic
- Automated backup scheduling
- Configurable backup workflows
- Maintainable and extensible architecture

## License

MIT License
