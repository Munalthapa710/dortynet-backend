using System.Text.Json;

namespace EmployeeApi.Logging
{
    public class FileAppLogger : IAppLogger
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILoggerSetting _loggerSetting;
        private readonly object _fileLock = new();
        private string _fileName = "employee-api";

        public FileAppLogger(IWebHostEnvironment environment, ILoggerSetting loggerSetting)
        {
            _environment = environment;
            _loggerSetting = loggerSetting;
        }

        public bool Log(LogType logType, Func<string> messageFunc, object? payload = null)
        {
            if (!_loggerSetting.AllowLogging)
            {
                return false;
            }

            try
            {
                var entry = new
                {
                    TimestampUtc = DateTime.UtcNow,
                    Level = logType.ToString(),
                    Message = messageFunc(),
                    Payload = CreatePayload(payload)
                };

                var line = JsonSerializer.Serialize(entry);

                lock (_fileLock)
                {
                    Directory.CreateDirectory(GetLogDirectory());
                    File.AppendAllText(GetLogFilePath(), line + Environment.NewLine);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CreateFile<T>()
        {
            _fileName = SanitizeFileName(typeof(T).Name);
            return true;
        }

        public bool CreateFile(string name)
        {
            _fileName = SanitizeFileName(name);
            return true;
        }

        private object? CreatePayload(object? payload)
        {
            if (payload is not Exception exception)
            {
                return payload;
            }

            return new
            {
                ExceptionType = exception.GetType().FullName,
                exception.Message,
                exception.StackTrace
            };
        }

        private string GetLogDirectory()
        {
            return Path.Combine(_environment.ContentRootPath, "Logs");
        }

        private string GetLogFilePath()
        {
            return Path.Combine(GetLogDirectory(), $"{_fileName}_{DateTime.UtcNow:yyyyMMdd}.log");
        }

        private static string SanitizeFileName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return "employee-api";
            }

            foreach (var invalidChar in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(invalidChar, '_');
            }

            return name;
        }
    }
}
