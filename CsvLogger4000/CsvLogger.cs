using Csv4000;
using System;
using System.Threading.Tasks;

namespace CsvLogger4000
{
    public class CsvLogger
    {
        private readonly CsvOf<LogEntry> csvFile;
        private readonly Func<string> GetUserName;
        public string loggerId = Guid.NewGuid().ToString();

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

        public void Log(string Message)
        {
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    var logEntry = new LogEntry
                    {
                        Message = Message
                    };

                    var user = GetUserName();
                    if (string.IsNullOrEmpty(user) == false)
                        logEntry.User = user;

                    await csvFile.WriteAsync(logEntry);
                }
                catch { }
            });

        }
    }
}
