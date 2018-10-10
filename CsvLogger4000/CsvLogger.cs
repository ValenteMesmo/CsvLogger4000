using Csv4000;
using System;
using System.Threading.Tasks;

namespace CsvLogger4000
{
    public partial class CsvLogger
    {
        private readonly CsvOf<LogEntry> csvFile;
        private readonly Func<string> GetUserName;
        public string loggerId = Guid.NewGuid().ToString();
        private static bool debugging = false;

        public CsvLogger(string filePath, Func<string> GetUserName = null)
        {
            csvFile = new CsvOf<LogEntry>(filePath);
            if (GetUserName == null)
                this.GetUserName = () => string.Empty;
            else
                this.GetUserName = GetUserName;
        }

        public void Clear()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    csvFile.Clear();
                }
                catch { }
            });
        }

        public void LogCritical(string message) => Log(message, LogSeverity.Critical);
        public void LogError(string message) => Log(message, LogSeverity.Error);
        public void LogWarning(string message) => Log(message, LogSeverity.Warning);
        public void LogInformational(string message) => Log(message, LogSeverity.Informational);
        public void LogDebug(string message) { if (debugging) Log(message, LogSeverity.Debug); }

        public void EnableDebugMode() => debugging = true;
        public void DisableDebugMode() => debugging = false;

        private void Log(string Message, LogSeverity severity)
        {
            try
            {
                var user = GetUserName();

                Task.Factory.StartNew(async () =>
                {
                    try
                    {
                        var logEntry = new LogEntry
                        {
                            Message = Message,
                            Id = loggerId,
                            Severity = severity
                        };

                        if (string.IsNullOrEmpty(user) == false)
                            logEntry.User = user;

                        await csvFile.WriteAsync(logEntry);
                    }
                    catch { }
                });

            }
            catch { }
        }

        public bool DebugModeEnabled()
        {
            return debugging;
        }
    }
}
