# GoogleAgentTextFormatter for Serilog

The `GoogleAgentTextFormatter` is a custom Serilog text formatter that formats log events in a way that is compatible with Google Cloud Platform (GCP) logging services. This formatter allows your application logs to be interpreted correctly by GCP solutions like Cloud Run, enabling seamless integration with Google Cloud Platform monitoring tools.

## Installation

To install the `Nandel.Extensions.Serilog.GoogleAgentTextFormatter` NuGet package, run the following command in your package manager console:

```bash
Install-Package Nandel.Extensions.Serilog.GoogleAgentTextFormatter
```

Alternatively, you can add it to your project file (.csproj) directly:

```xml
<PackageReference Include="Nandel.Extensions.Serilog.GoogleAgentTextFormatter" Version="1.0.0-alpha.0" />
```

## Usage

### 1. In Code

To use the `GoogleAgentTextFormatter` in your application, you need to configure Serilog to use this formatter. Here’s an example of how to set it up in your code:

```csharp
using Serilog;

class Program
{
    static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(new GoogleAgentTextFormatter()) // Updated formatter
            .CreateLogger();

        Log.Information("This is an information log.");
        Log.Error("This is an error log with an exception.", new Exception("Sample exception"));

        Log.CloseAndFlush();
    }
}
```

### 2. Using App Settings

You can also configure the `GoogleAgentTextFormatter` through your application settings (e.g., `appsettings.json`). Here’s how you can do it:

```json
{
  "Serilog": {
    "Using": [ "Nandel.Extensions.Serilog.GoogleAgentTextFormatter" ],
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "formatter": "Nandel.Extensions.Serilog.GoogleAgentTextFormatter, Nandel.Extensions.Serilog.GoogleAgentTextFormatter"
        }
      }
    ]
  }
}
```

## Summary

The `GoogleAgentTextFormatter` is designed to ensure that your application's log output is formatted in a way that is compatible with GCP's logging services. By using this formatter, you can easily integrate your application logs with Google Cloud Platform monitoring solutions, allowing for better observability and management of your cloud applications.
