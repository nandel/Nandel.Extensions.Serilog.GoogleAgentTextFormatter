// See https://aka.ms/new-console-template for more information

using Nandel.Extensions.Serilog;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.Console(new GoogleAgentTextFormatter()) // Updated formatter
    .CreateLogger();

// The basic
Log.Verbose("Verbose, Serilog!");
Log.Debug("Debug, Serilog!");
Log.Information("Hello, Serilog!");
Log.Warning("Warning, Serilog!");
Log.Error("Error, Serilog!");
Log.Fatal("Fatal, Serilog!");

// Templated
Log.Verbose("{Level}, Serilog!", "Verbose");
Log.Debug("{Level}, Serilog!", "Debug");
Log.Information("{Level}, Serilog!", "Information");
Log.Warning("{Level}, Serilog!", "Warning");
Log.Error("{Level}, Serilog!", "Error");
Log.Fatal("{Level}, Serilog!", "Fatal");

// Templated With Json Forbidden Chars
Log.Verbose("\"{Level}\", Serilog!", "Verbose");
Log.Debug("\"{Level}\", Serilog!", "Debug");
Log.Information("\"{Level}\", Serilog!", "Information");
Log.Warning("\"{Level}\", Serilog!", "Warning");
Log.Error("\"{Level}\", Serilog!", "Error");
Log.Fatal("\"{Level}\", Serilog!", "Fatal");

// Templated with Structured Object
Log.Verbose("{@Game}, Serilog!", new Game("Split Fiction", 2025));
Log.Debug("{@Game}, Serilog!", new Game("Baldur's Gate 3", 2023));
Log.Information("{@Game}, Serilog!", new Game("The Witcher 3", 2019));
Log.Warning("{@Game}, Serilog!", new Game("GTA Online", 2013));
Log.Error("{@Game}, Serilog!", new Game("The Elder Scrolls V: Skyrim", 2016));
Log.Fatal("{@Game}, Serilog!", new Game("Fortnite", 2017));

public record Game(string Name, int Year);