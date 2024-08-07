using System.Text;

namespace BannedLogger
{
    public class Logger
    {
        private readonly string _name;
        private readonly string _baseDirectory;
        private readonly bool   _isWriteOnConsole;

        private string _currentLogFile = string.Empty;

        public Logger(string name, string baseDirectory, bool writeOnConsole = false)
        {
            _name             = name;
            _baseDirectory    = baseDirectory;
            _isWriteOnConsole = writeOnConsole;
        }

        private void SetupLogFile()
        {
            var currentMonth   = DateTime.Now.ToString("yyyy-MM");
            var monthDirectory = Path.Combine(_baseDirectory, currentMonth);
            if (!Directory.Exists(monthDirectory))
            {
                Directory.CreateDirectory(monthDirectory);
            }

            var currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            _currentLogFile = Path.Combine(monthDirectory, $"{currentDate}.log");
        }

        private void Log(LogLevel level, string message)
        {
            SetupLogFile();
            var now = DateTime.Now;

            var finalMessage = $"{now:yyyy-MM-dd H:mm:ss} - {level} - {_name} - {message}";

            if (_isWriteOnConsole)
            {
                Console.WriteLine(finalMessage);
            }

            using var writer = new StreamWriter(_currentLogFile, true, Encoding.UTF8);
            writer.WriteLine(finalMessage);
        }

        public void Debug(string message)
        {
            Log(LogLevel.Debug, message);
        }

        public void Info(string message)
        {
            Log(LogLevel.Info, message);
        }

        public void Warning(string message)
        {
            Log(LogLevel.Warning, message);
        }

        public void Error(string message)
        {
            Log(LogLevel.Error, message);
        }
    }
}