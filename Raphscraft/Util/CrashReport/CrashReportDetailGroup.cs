namespace Raphscraft.Util.CrashReport;

public class CrashReportDetailGroup {
    public string Name { get; set; }
    public List<Func<Exception, string>> Details { get; set; }
}