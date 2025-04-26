namespace Banned.Logger;

public class LoggerOptions
{
    public string   Name             { get; set; } = string.Empty;
    public string   BaseDirectory    { get; set; } = string.Empty;
    public bool     WriteOnConsole   { get; set; }
    public LogLevel MinimumLevel     { get; set; } = LogLevel.Debug;
    public long     MaxFileSize      { get; set; } = 10 * 1024 * 1024; // 10MB
    public int      MaxRetainedFiles { get; set; } = 30;               // 保留30天的日志
    public string   LogFormat        { get; set; } = "{timestamp:yyyy-MM-dd HH:mm:ss} - {level} - {name} - {message}";
}