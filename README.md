# Banned.Logger

[中文文档](https://github.com/banned2054/Banned.Logger/blob/master/Docs/README.zh-CN.md)

A simple and efficient .NET logging library with file logging and log rotation support.

## Features

- 📝 Multiple log levels (Debug, Info, Warning, Error)
- 📂 Automatic log file organization by month and date
- 🔄 Automatic log rotation to prevent oversized files
- 🧹 Automatic cleanup of expired log files
- 🎨 Customizable log format
- 💻 Console output support
- 🔒 Thread-safe design
- ⚡ Asynchronous writing without affecting main program performance

## Installation

```bash
dotnet add package Banned.Logger
```

## Quick Start

```csharp
using Banned.Logger;

// Create logger with configuration
var logger = new Logger(options =>
{
    options.Name = "MyApp";
    options.BaseDirectory = "logs";
    options.WriteOnConsole = true;
    options.MinimumLevel = LogLevel.Info;
    options.MaxFileSize = 5 * 1024 * 1024;  // 5MB
    options.MaxRetainedFiles = 7;           // Keep logs for 7 days
    options.LogFormat = "[{timestamp}] [{level}] {name}: {message}";
});

// Log messages asynchronously
await logger.InfoAsync("This is an info message");
await logger.WarningAsync("This is a warning message");
await logger.ErrorAsync("This is an error message");
```

## Configuration Options

| Option | Type | Default | Description |
|--------|------|---------|-------------|
| Name | string | "" | Logger name |
| BaseDirectory | string | "" | Base directory for log files |
| WriteOnConsole | bool | false | Whether to output to console |
| MinimumLevel | LogLevel | LogLevel.Debug | Minimum log level |
| MaxFileSize | long | 10MB | Maximum size of a single log file |
| MaxRetainedFiles | int | 30 | Number of days to retain log files |
| LogFormat | string | "{timestamp:yyyy-MM-dd HH:mm:ss} - {level} - {name} - {message}" | Log format template |

## Log Format

The log format supports the following placeholders:
- `{timestamp}`: Timestamp
- `{level}`: Log level
- `{name}`: Logger name
- `{message}`: Log message

## Log File Organization

Log files are organized in the following structure:
```
{BaseDirectory}/{yyyy-MM}/{yyyy-MM-dd}.log
```

Example:
```
logs/
  ├── 2024-03/
  │   ├── 2024-03-15.log
  │   └── 2024-03-15_153045.log  (rotated file)
  └── 2024-04/
      └── 2024-04-01.log
```

## Log Rotation

When a log file reaches `MaxFileSize`, it will be automatically rotated:
1. The current file is renamed to `{date}_{timestamp}.log`
2. A new log file is created for continued writing

## Notes

1. Ensure the application has write permissions for the log directory
2. Adjust `MaxFileSize` and `MaxRetainedFiles` parameters according to your needs
3. Log rotation and cleanup operations are asynchronous and won't block the main program
4. All logging operations are asynchronous, use `await` when calling logging methods

## License

This project is licensed under the Apache License 2.0.
