namespace Raphscraft.Client.Render;

/// <summary>
/// GPU kind
/// </summary>
public enum GpuKind {
    /// <summary>
    /// The GPU is external to the CPU silicon.
    /// </summary>
    Discrete,
    
    /// <summary>
    /// The GPU is built into the CPU's silicon
    /// </summary>
    Integrated,
    
    /// <summary>
    /// GPUs found in virtual machines.
    /// </summary>
    Virtual,
    
    /// <summary>
    /// The CPU emulates the GPU.
    /// </summary>
    SoftwareRenderer
}

/// <summary>
/// Representation of a Graphics Processing Unit.
/// </summary>
public abstract class Gpu {
    /// <summary>
    /// The GPU's name (example: "Intel(R) UHD Graphics 620 (KBL GT2)")
    /// </summary>
    public abstract string Name { get; }
    
    /// <summary>
    /// The GPU's driver (example: "Mesa 25.0.2")
    /// </summary>
    public abstract string Driver { get; }

    /// <summary>
    /// The GPU's kind.
    /// </summary>
    public abstract GpuKind Kind { get; }

    /// <summary>
    /// Gives a score to the GPU.
    /// </summary>
    /// <returns></returns>
    public int GetScore() => Kind switch {
            GpuKind.Discrete => 5000,
            GpuKind.Integrated => 2500,
            GpuKind.Virtual => 1500,
            GpuKind.SoftwareRenderer => 200,
            _ => 100
        };
}