using System;

namespace CsvLogger4000
{
    public class LogEntry
    {
        public DateTime Date { get; } = DateTime.Now;
        public LogSeverity Severity { get; set; } = LogSeverity.Informational;
        public string User { get; set; }
        public string Message { get; set; }
        public string Id { get; set; } 
    }
}
