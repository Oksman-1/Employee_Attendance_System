using Serilog.Core;
using Serilog.Events;

namespace Attendance.Shared.SerilogEnricher;

public class ShortSourceContextEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (!logEvent.Properties.TryGetValue("SourceContext", out var value)) return;
        var fullName = value.ToString().Trim('"');
        
        // take only class name
        var shortName = fullName.Split('.').Last(); 
        var prop = propertyFactory.CreateProperty("ShortSourceContext", shortName);
        logEvent.AddOrUpdateProperty(prop);
    }
}