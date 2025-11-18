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
/// GPU vendors, mapped from their PCI ID
/// </summary>
public enum GpuVendor {
    Intel,
    Amd,
    Nvidia,
    Mesa // if using Lavapipe/LLVMpipe.
}

/// <summary>
/// Class representing a GPU.
/// </summary>
public abstract class Gpu {
    /// <summary>
    /// The GPU's name (example: "Intel(R) UHD Graphics 620 (KBL GT2)"
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

    public int GetScore() {
        var score = 0;

        switch (Kind) {
            case GpuKind.Discrete:         score += 5000; break;
            case GpuKind.Integrated:       score += 2500; break;
            case GpuKind.Virtual:          score += 1500; break;
            case GpuKind.SoftwareRenderer: score += 200; break;
            default:                       score += 100; break;
        }
        
        return score;
    }
}