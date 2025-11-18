namespace Raphscraft.Util;

using Raphscraft.Client.Render;
using System.Globalization;
using System.Security.Cryptography;

public enum LogType {
    Error,
    Warning,
    Info,
}

public class Logger {
    private static Dictionary<LogType, Color> _logTypeColorMapping = new() { 
        { LogType.Error,   Color.FromHsl(0, 160 / 255.0f, 100 / 255.0f) },
        { LogType.Warning, Color.FromHsl(60, 160 / 255.0f, 255 / 255.0f) },
        { LogType.Info,    Color.FromHsl(170, 160 / 255.0f, 100 / 255.0f) }
    };

    public string Category { get; private set; }
    
    private Logger(string? category) {
        Category = category ?? "Client";
    }

    public static Logger Open(string? category = null) =>
        new(category);
    
    public void Log(LogType logType, string message) =>
        Console.WriteLine(
            $"{Thread.CurrentThread.Name} ) " +
            $"{DateTime.Now.ToString(CultureInfo.InvariantCulture)} {Category} " +
            AnsiEscapeCodes.GetForColor(_logTypeColorMapping[logType]) + 
            $"{logType.ToString()}" + 
            AnsiEscapeCodes.SgrReset +
            $": {message}");
    
    public void Error(string message)   => Log(LogType.Error, message);
    public void Warning(string message) => Log(LogType.Warning, message);
    public void Info(string message)    => Log(LogType.Info, message);
    
}