using Raphscraft.Client;
using Raphscraft.Util;
using Raphscraft.Util.CrashReport;

AppDomain.CurrentDomain.UnhandledException += (s, e) => {
    /*if (e.ExceptionObject is Exception ex) {
        Console.WriteLine($"{ex.GetType().FullName}: {(ex.Message)}");

        var stackTrace = new StackTrace(ex, true);
        foreach (var frame in stackTrace.GetFrames()) {
            // Method name
            var method = frame.GetMethod();
            if (method == null) {
                Console.WriteLine($"- Unknown");
                continue;
            }

            Console.WriteLine($"- {method} in {method.DeclaringType?.FullName ?? "Unknown"}");

            // File
            Console.WriteLine($"  └ Declared in {frame.GetFileName()} (line {frame.GetFileLineNumber()}, column {frame.GetFileColumnNumber()})");
        }
    }*/

    Logger.Open("Raph's Craft entry point").Error("Caught an unhandled exception.");
    
    new CrashReportEmitter().Emit(e.ExceptionObject as Exception ?? new("Unknown exception... ?"));
    Environment.Exit(1);
};

Thread.CurrentThread.Name = "Client thread";

new RaphscraftClient().Run();