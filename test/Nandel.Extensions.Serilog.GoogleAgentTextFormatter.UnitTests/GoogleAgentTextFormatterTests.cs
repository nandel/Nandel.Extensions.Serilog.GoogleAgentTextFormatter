using Newtonsoft.Json.Linq;
using Serilog.Events;
using Serilog.Parsing;
using Shouldly;

#pragma warning disable CS8604 // We need to assert some nullables in JObject, its working as intended.

namespace Nandel.Extensions.Serilog.UnitTests;

public class GoogleAgentTextFormatterTests
{
    [Fact]
    public void Format_ShouldSerializeLogEventCorrectly()
    {
        // Arrange
        var formatter = new GoogleAgentTextFormatter();
        var logEvent = new LogEvent(
            DateTimeOffset.UtcNow,
            LogEventLevel.Information,
            null,
            new MessageTemplateParser().Parse("Test message"),
            new List<LogEventProperty>
            {
                new("simple", new ScalarValue("value")),
            });

        var output = new StringWriter();

        // Act
        formatter.Format(logEvent, output);

        // Assert
        var result = output.ToString();
        var parsedJson = JObject.Parse(result);

        parsedJson["severity"].Value<string?>().ShouldBe("INFO");
        parsedJson["message"].Value<string?>().ShouldBe("Test message");
        parsedJson["exception"].Value<string?>().ShouldBeNull();
    }

    [Theory]
    [InlineData(LogEventLevel.Verbose, "DEBUG")]
    [InlineData(LogEventLevel.Debug, "DEBUG")]
    [InlineData(LogEventLevel.Information, "INFO")]
    [InlineData(LogEventLevel.Warning, "WARNING")]
    [InlineData(LogEventLevel.Error, "ERROR")]
    [InlineData(LogEventLevel.Fatal, "CRITICAL")]
    public void Format_ShouldMapSeverityLevelsCorrectly(LogEventLevel serilogLevel, string expectedSeverity)
    {
        // Arrange
        var formatter = new GoogleAgentTextFormatter();
        var logEvent = new LogEvent(
            DateTimeOffset.UtcNow,
            serilogLevel,
            null,
            new MessageTemplate("Severity test message", new List<MessageTemplateToken>()),
            new List<LogEventProperty>());

        var output = new StringWriter();

        // Act
        formatter.Format(logEvent, output);

        // Assert
        var result = output.ToString();
        var parsedJson = JObject.Parse(result);

        parsedJson["severity"].ShouldBe(expectedSeverity);
    }
}