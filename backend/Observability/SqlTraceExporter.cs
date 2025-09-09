using System.Diagnostics;
using System.Text.Json;
using OpenTelemetry;
using OpenTelemetry.Trace;
using backend.Data;

namespace backend.Observability
{
    public class SqlTraceExporter : BaseExporter<Activity>
    {
        private readonly TraceDbContext _db;

        public SqlTraceExporter(TraceDbContext db)
        {
            _db = db;
        }

        public override ExportResult Export(in Batch<Activity> batch)
        {
            foreach (var activity in batch)
            {
                var entity = new TraceEntity
                {
                    TraceId = activity.TraceId.ToString(),
                    SpanId = activity.SpanId.ToString(),
                    Operation = activity.DisplayName,
                    StartTime = activity.StartTimeUtc,
                    Duration = activity.Duration,
                    AttributesJson = JsonSerializer.Serialize(activity.TagObjects)
                };
                _db.Traces.Add(entity);
            }
            _db.SaveChanges();
            return ExportResult.Success;
        }
    }
}
