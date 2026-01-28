using Serilog;

namespace Attendance.Shared.SerilogEnricher;

public static class LoggingConfiguration
{
    public static void Configure()
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.With<ShortSourceContextEnricher>()
            .MinimumLevel.Information()
            //.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            //.MinimumLevel.Override("System", LogEventLevel.Warning)

            // Exclude only Swagger HTTP request logs
            .Filter.ByExcluding(le =>
                le.Properties.ContainsKey("SourceContext") &&
                le.Properties["SourceContext"].ToString().Contains("Diagnostics") &&
                le.RenderMessage().Contains("/swagger/")
            )

            .WriteTo.Console(
                outputTemplate: "[{Timestamp:dd-MMM-yyyy:hh:mm:ss tt} {Level:u3}-API===>{ShortSourceContext}] {Message:lj}{NewLine}{Exception}"
            )
            .WriteTo.File("Logs/log--.txt", rollingInterval: RollingInterval.Day, outputTemplate: "[{Timestamp:dd-MMM-yyyy:hh:mm:ss tt} {Level:u3}-API===>{ShortSourceContext}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();
    }
}