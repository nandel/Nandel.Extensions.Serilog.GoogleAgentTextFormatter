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
        output.Write("{");

        // Main Properties
        output.Write($"\"timestamp\": \"{logEvent.Timestamp.UtcDateTime:o}\"");
        output.Write($", \"severity\": \"{s_severityLevels[logEvent.Level]}\"");
        output.Write($", \"message\": \"{EscapeJsonString(logEvent.RenderMessage())}\"");
        output.Write($", \"exception\": {GetNullableString(logEvent.Exception)}");
        
        // Labels
        output.Write( ", \"logging.googleapis.com/labels\": {");
        
        output.Write($" \"message_template\": \"{EscapeJsonString(logEvent.MessageTemplate.Text)}\"");
        output.Write($", \"trace_id\": {GetNullableString(logEvent.TraceId)}"); // Maybe we can integrate with the property built in GCP?
        output.Write($", \"span_id\": {GetNullableString(logEvent.SpanId)}");// Maybe we can integrate with the property built in GCP?
        
        // Removed for now... need some work
        // In the MVP using serialization was easy...
        // foreach (var label in logEvent.Properties)
        // {
        //     output.Write($", \"{label.Key}\": ");
        //     label.Value.Render(output);
        // }
        
        output.Write("}"); // End labels
        
        // End Log Event
        output.Write("}"); 
        
        // Finish with a new line
        output.WriteLine();
    }

    private static string GetNullableString(object? value)
    {
        if (value is not null)
        {
            return $"\"{EscapeJsonString(value?.ToString())}\"";
        }

        return "null";
    }
    
    private static string EscapeJsonString(string? str)
    {
        if (string.IsNullOrEmpty(str)) return "null";
        
        return str
            .Replace("\\", "\\\\")  // Escape backslash
            .Replace("\"", "\\\"")  // Escape quotes
            .Replace("\b", "\\b")   // Escape backspace
            .Replace("\f", "\\f")   // Escape form feed
            .Replace("\n", "\\n")   // Escape newline
            .Replace("\r", "\\r")   // Escape carriage return
            .Replace("\t", "\\t");  // Escape tab
    }

}