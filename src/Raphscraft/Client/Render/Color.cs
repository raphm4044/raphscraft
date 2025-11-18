namespace Raphscraft.Client.Render;

/// <summary>
/// Represents a color in the RGBA 0.0 - 1.0 format.
/// </summary>
public class Color {
    /// <summary>
    /// Opacity.
    /// </summary>
    public float Opacity { get; private set; } = 1.0f;
    /// <summary>
    /// Red intensity
    /// </summary>
    public float Red { get; private set; }
    /// <summary>
    /// Green intensity.
    /// </summary>
    public float Green { get; private set; }
    /// <summary>
    /// Blue intensity.
    /// </summary>
    public float Blue { get; private set; }

    private Color() {}
    
    /// <summary>
    /// Get a <see cref="Color"/> instance from H.S.L. values.
    /// </summary>
    /// <param name="red">Red intensity</param>
    /// <param name="green">Green intensity</param>
    /// <param name="blue">Blue intensity</param>
    /// <returns>A <see cref="Color"/> instance</returns>
    public static Color FromRgb(float red, float green, float blue)
        => FromRgb(1.0f, red, green, blue);
    
    /// <summary>
    /// Get a <see cref="Color"/> instance from H.S.L. values.
    /// </summary>
    /// <param name="opacity">Opacity</param>
    /// <param name="red">Red intensity</param>
    /// <param name="green">Green intensity</param>
    /// <param name="blue">Blue intensity</param>
    /// <returns>A <see cref="Color"/> instance</returns>
    public static Color FromRgb(float opacity, float red, float green, float blue) {
        return new() {
            Opacity = Math.Clamp(opacity, 0.0f, 1.0f),
            Red = Math.Clamp(red, 0.0f, 1.0f),
            Green = Math.Clamp(green, 0.0f, 1.0f),
            Blue = Math.Clamp(blue, 0.0f, 1.0f),
        };
    }
    
    /// <summary>
    /// Get a <see cref="Color"/> instance from H.S.L. values.
    /// </summary>
    /// <param name="opacity">Opacity (range: 0.0-1.0)</param>
    /// <param name="hue">Hue (range: 0.0-360.0)</param>
    /// <param name="saturation">Saturation (range: 0.0-1.0)</param>
    /// <param name="lightness">Lightness (range: 0.0-1.0)</param>
    /// <returns>A <see cref="Color"/> instance.</returns>
    public static Color FromHsl(float opacity, float hue, float saturation, float lightness)
    {
        hue %= 360.0f;
        saturation = Math.Clamp(saturation, 0.0f, 1.0f);
        lightness = Math.Clamp(lightness, 0.0f, 1.0f);
        opacity = Math.Clamp(opacity, 0.0f, 1.0f);

        if (saturation == 0.0f)
            return FromRgb(opacity, lightness, lightness, lightness); // Gris

        var c = (1.0f - Math.Abs(2.0f * lightness - 1.0f)) * saturation;
        var m = lightness - c / 2.0f;
        var x = c * (1.0f - Math.Abs((hue / 60.0f % 2.0f) - 1.0f));

        float r1 = 0.0f, g1 = 0.0f, b1 = 0.0f;
        var hPrime = hue / 60.0f;

        switch (hPrime) {
            case >= 0.0f and < 1.0f: r1 = c; g1 = x; break;
            case >= 1.0f and < 2.0f: r1 = x; g1 = c; break;
            case >= 2.0f and < 3.0f: g1 = c; b1 = x; break;
            case >= 3.0f and < 4.0f: g1 = x; b1 = c; break;
            case >= 4.0f and < 5.0f: r1 = x; b1 = c; break;
            default: r1 = c; b1 = x; break;
        }

        return FromRgb(
            opacity,
            r1 + m,
            g1 + m,
            b1 + m);
    }

    /// <summary>
    /// Get a <see cref="Color"/> instance from H.S.L. values, with Opacity being 1.0f.
    /// </summary>
    /// <param name="hue">Hue (range: 0.0-360.0)</param>
    /// <param name="saturation">Saturation (range: 0.0-1.0)</param>
    /// <param name="lightness">Lightness (range: 0.0-1.0)</param>
    /// <returns>A <see cref="Color"/> instance.</returns>
    public static Color FromHsl(float hue, float saturation, float lightness)
        => FromHsl(1.0f, hue, saturation, lightness);
}