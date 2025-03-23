using Serilog.Events;
using Serilog.Formatting;

namespace Nandel.Extensions.Serilog;

public class GoogleAgentTextFormatter : ITextFormatter
{
    private static readonly Dictionary<LogEventLevel, string> s_severityLevels = new()
    {
        [LogEventLevel.Verbose] = "DEBUG",
        [LogEventLevel.Debug] = "DEBUG",
        [LogEventLevel.Information] = "INFO",
        [LogEventLevel.Warning] = "WARNING",
        [LogEventLevel.Error] = "ERROR",
        [LogEventLevel.Fatal] = "CRITICAL"
    };
    
    public void Format(LogEvent logEvent, TextWriter output)
    {
        // Start Log Event
        output.Write("{ ");

        // Main Properties
        output.Write($"  \"timestamp\": \"{logEvent.Timestamp.UtcDateTime:o}\" ");
        output.Write($", \"severity\": \"{s_severityLevels[logEvent.Level]}\" ");
        output.Write($", \"message\": \"{logEvent.RenderMessage()}\" ");
        output.Write($", \"exception\": {GetNullableString(logEvent.Exception)} ");
        
        // Labels
        output.Write( ", \"logging.googleapis.com/labels\": { ");
        
        output.Write($"  \"message_template\": \"{logEvent.MessageTemplate.Text}\" ");
        output.Write($", \"trace_id\": {GetNullableString(logEvent.TraceId)} "); // Maybe we can integrate with the property built in GCP?
        output.Write($", \"span_id\": {GetNullableString(logEvent.SpanId)} ");// Maybe we can integrate with the property built in GCP?
        
        foreach (var label in logEvent.Properties)
        {
            output.Write($", \"{label.Key}\": ");
            label.Value.Render(output);
        }
        
        output.Write("} "); // End labels
        
        // End Log Event
        output.Write("} "); 
        
        // Finish with a new line
        output.WriteLine();
    }

    private static string GetNullableString(object? value)
    {
        return value is null ? "null" : $"\"{value}\"";
    }
}