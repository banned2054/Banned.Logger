using System.Text;
using System.Text.RegularExpressions;

namespace Banned.Logger;

public class Logger
{
    private readonly LoggerOptions _options;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    private string _currentLogFile = string.Empty;

    public Logger(Action<LoggerOptions>? configure = null)
    {
        _options = new LoggerOptions();
        configure?.Invoke(_options);
    }

    private async Task SetupLogFileAsync()
    {
        var currentMonth   = DateTime.Now.ToString("yyyy-MM");
        var monthDirectory = Path.Combine(_options.BaseDirectory, currentMonth);
        if (!Directory.Exists(monthDirectory))
        {
            Directory.CreateDirectory(monthDirectory);
        }

        var currentDate = DateTime.Now.ToString("yyyy-MM-dd");
        var newLogFile  = Path.Combine(monthDirectory, $"{currentDate}.log");

        await _semaphore.WaitAsync();
        try
        {
            _currentLogFile = newLogFile;

            // 检查文件大小并处理轮转
            if (File.Exists(_currentLogFile))
            {
                var fileInfo = new FileInfo(_currentLogFile);
                if (fileInfo.Length >= _options.MaxFileSize)
                {
                    var timestamp   = DateTime.Now.ToString("HHmmss");
                    var newFileName = Path.Combine(monthDirectory, $"{currentDate}_{timestamp}.log");
                    File.Move(_currentLogFile, newFileName);
                }
            }

            // 清理旧日志文件
            await CleanupOldLogsAsync();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task CleanupOldLogsAsync()
    {
        try
        {
            var cutoffDate  = DateTime.Now.AddDays(-_options.MaxRetainedFiles);
            var allLogFiles = Directory.GetFiles(_options.BaseDirectory, "*.log", SearchOption.AllDirectories);

            foreach (var file in allLogFiles)
            {
                var fileInfo = new FileInfo(file);
                if (fileInfo.LastWriteTime < cutoffDate)
                {
                    await Task.Run(() => File.Delete(file));
                }
            }
        }
        catch (Exception ex)
        {
            // 记录清理过程中的错误，但不影响主流程
            Console.WriteLine($"Error cleaning up old logs: {ex.Message}");
        }
    }

    private async Task LogAsync(LogLevel level, string message)
    {
        if (level < _options.MinimumLevel)
        {
            return;
        }

        await SetupLogFileAsync();
        var now = DateTime.Now;

        // 处理时间戳格式化
        var finalMessage = Regex.Replace(_options.LogFormat, @"\{timestamp:(.*?)\}", match =>
        {
            var format = match.Groups[1].Value;
            return now.ToString(format);
        });

        finalMessage = finalMessage
                      .Replace("{level}", level.ToString())
                      .Replace("{name}", _options.Name)
                      .Replace("{message}", message);

        if (_options.WriteOnConsole)
        {
            Console.WriteLine(finalMessage);
        }

        await WriteToFileAsync(finalMessage);
    }

    private async Task WriteToFileAsync(string message)
    {
        await _semaphore.WaitAsync();
        try
        {
            await File.AppendAllTextAsync(_currentLogFile, message + Environment.NewLine, Encoding.UTF8);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task DebugAsync(string message)
    {
        await LogAsync(LogLevel.Debug, message);
    }

    public async Task InfoAsync(string message)
    {
        await LogAsync(LogLevel.Info, message);
    }

    public async Task WarningAsync(string message)
    {
        await LogAsync(LogLevel.Warning, message);
    }

    public async Task ErrorAsync(string message)
    {
        await LogAsync(LogLevel.Error, message);
    }
}