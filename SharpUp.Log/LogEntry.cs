using System;
using System.Collections.Generic;
using System.Text;

namespace SharpUp.Log
{
    public class LogEntry
    {
        public LogEntry(LogType type, string message)
        {
            Type = type;
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public LogType Type { get; set; }
        public string Message { get; set; }

        public DateTime Time { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return string.Format("{0:dd-MM-yyyy HH:mm:ss.fff}:[{1}] - {2}", Time, Type.ToString(), Message);
        }
    }
}