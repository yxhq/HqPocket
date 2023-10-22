using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace HqPocket.Extensions.Logging
{
    public class LogItem
    {
        public string? ClassName { get; set; }
        public EventId? EventId { get; set; }
        public DateTime? DateTime { get; set; }
        public LogLevel LogLevel { get; set; } = LogLevel.Information;
        public string Message { get; set; } = string.Empty;

        public override string ToString()
        {
            StringBuilder sb = new();

            if (EventId is not null)
            {
                sb.Append($"[{EventId}]  ");
            }

            if (DateTime is not null)
            {
                sb.Append($"{DateTime:yyyy-MM-dd HH:mm:ss:fff}  ");
            }

            if (ClassName is not null)
            {
                sb.Append(ClassName).Append("  ");
            }

            return sb.Append(Message).ToString();
        }
    }
}
