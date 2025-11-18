namespace Raphscraft.Util.CrashReport;

using System.Diagnostics;
using System.Runtime.InteropServices;
using Raphscraft.Util.CrashReport.Output;
using System.Text;
using HarmonyLib;

/// <summary>
/// Emits a crash report.
/// </summary>
public class CrashReportEmitter {
    public List<CrashReportDetailGroup> DetailGroups { get; private set; } = [
        new() {
            Name = "What happened",
            Details = [
                (e) => {
                    StringBuilder stringBuilder = new();
                    stringBuilder.AppendLine($"{e.GetType().FullName}: {(e.Message)}");

                    var stackTrace = new StackTrace(e, true);
                    foreach (var frame in stackTrace.GetFrames()) {
                        var method = frame.GetMethod();
                        if (method == null) {
                            Console.WriteLine($"- Unknown");
                            continue;
                        }

                        stringBuilder.AppendLine($"- {method} in {method.DeclaringType?.FullName ?? "Unknown"}");
                        if (frame.GetFileName() != null)
                            stringBuilder.AppendLine($"  └ Declared in {frame.GetFileName()} (line {frame.GetFileLineNumber()}, column {frame.GetFileColumnNumber()})");
                    }
                    
                    return stringBuilder.ToString();
                }
            ]
        },
        new() {
            Name = "Environment",
            Details = [
                (e) => $"Operating system: {RuntimeInformation.RuntimeIdentifier} {Environment.OSVersion.Version}",
                (e) => $"O.S. architecture: {RuntimeInformation.OSArchitecture}",
                (e) => $".NET CLR version: {Environment.Version.ToString()}",
                (e) => $"Managed memory usage: {GC.GetTotalMemory(false) / (1024 * 1024)}MiB of {Environment.WorkingSet / (1024 * 1024)}MiB",
                (e) => {
                    var patchCount = Harmony.GetAllPatchedMethods().Count();
                    return patchCount > 0
                        ? $"Patched: Yes (Harmony patched {patchCount} methods)"
                        : "Patched: No (Harmony patched nothing)";
                }
            ]
        }
    ];

    public List<Action<string>> Outputs { get; private set; } = [
        ConsoleOutput.Output
    ];
    
    public void Emit(Exception ex) {
        StringBuilder sb = new();

        var woozySentenceList = CrashReportWoozySentences.NormalWoozySentences;
        if (ex.GetType() == typeof(AccessViolationException))
            woozySentenceList = CrashReportWoozySentences.SegfaultWoozySentences;
        
        sb.AppendLine("== Raph's Craft crashed !");
        sb.AppendLine($"# {woozySentenceList[Random.Shared.Next(woozySentenceList.Count)]}");
        sb.AppendLine();

        foreach (var detailGroup in DetailGroups) {
            sb.AppendLine($"- {detailGroup.Name} -");
            foreach (var detail in detailGroup.Details)
                sb.AppendLine(detail(ex));
            sb.AppendLine();
        }
        
        // Emit to outputs
        foreach (var output in Outputs)
            output(sb.ToString());
    }
}