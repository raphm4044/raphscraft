namespace Raphscraft.Util.CrashReport;

/// <summary>
/// Provides lists of funny sentences that are displayed in a crash report.
/// </summary>
public static class CrashReportWoozySentences {
    public readonly static List<string> SegfaultWoozySentences = [
        "At least you got this instead of a \"Segmentation fault (core dumped)\"",
        "Vulkan fait surement des siennes.",
        "Don't try to write at NULL the next time!"
    ];
    
    public readonly static List<string> NormalWoozySentences = [
        "Fuck, I exploded!",
        "Pardon."
    ];
}