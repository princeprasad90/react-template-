using System;

namespace backend.Data
{
    public class TraceEntity
    {
        public int Id { get; set; }
        public string TraceId { get; set; } = string.Empty;
        public string SpanId { get; set; } = string.Empty;
        public string Operation { get; set; } = string.Empty;
        public DateTimeOffset StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public string AttributesJson { get; set; } = string.Empty;
    }
}
