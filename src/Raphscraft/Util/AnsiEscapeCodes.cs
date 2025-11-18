namespace Raphscraft.Util;

using Client.Render;

public class AnsiEscapeCodes {
    public const string SgrReset = "\e[0m";

    public static string GetForColor(Color color) =>
        $"\e[38;2;" +
        $"{(int)(color.Red * 255.0)};" +
        $"{(int)(color.Green * 255.0)};" +
        $"{(int)(color.Blue * 255.0)}m";

}