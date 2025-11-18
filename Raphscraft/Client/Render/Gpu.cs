namespace Raphscraft.Client.Render;

public enum DriverId : int
{
    /// <summary>Vulkan software renderer provided by Google.</summary>
    GoogleSwiftshader = 10,
    /// <summary>Mesa's Vulkan software renderer using LLVM (Lavapipe/llvmpipe).</summary>
    MesaLlvmpipe = 13,

    /// <summary>Mesa driver for AMD Radeon GPUs (Radeon Vulkan).</summary>
    MesaRadv = 3,
    /// <summary>Mesa driver for Intel GPUs (Open-source).</summary>
    IntelOpenSourceMesa = 6,
    /// <summary>Mesa driver for Qualcomm Adreno GPUs (Turnip).</summary>
    MesaTurnip = 18,
    /// <summary>Mesa driver for Raspberry Pi GPUs (V3D Vulkan).</summary>
    MesaV3DV = 19,
    /// <summary>Mesa driver for Arm Mali Midgard/Bifrost GPUs (Panfrost Vulkan).</summary>
    MesaPanvk = 20,
    /// <summary>Mesa driver for VirtIO GPUs (Venus).</summary>
    MesaVenus = 22,
    /// <summary>Mesa driver for Microsoft D3D12/DirectX 12 (Dozen).</summary>
    MesaDozen = 23,
    /// <summary>Mesa driver for NVIDIA GPUs (NVK).</summary>
    MesaNvk = 24,
    /// <summary>Mesa driver for Imagination PowerVR GPUs (Open-source).</summary>
    ImaginationOpenSourceMesa = 25,
    /// <summary>Mesa driver for Apple Silicon and M-series GPUs (Honeykrisp).</summary>
    MesaHoneykrisp = 26,

    /// <summary>Proprietary driver for AMD GPUs.</summary>
    AmdProprietary = 1,
    /// <summary>Proprietary driver for NVIDIA GPUs.</summary>
    NvidiaProprietary = 4,
    /// <summary>Proprietary driver for Intel GPUs on Windows.</summary>
    IntelProprietaryWindows = 5,
    /// <summary>Proprietary driver for Imagination PowerVR GPUs.</summary>
    ImaginationProprietary = 7,
    /// <summary>Proprietary driver for Qualcomm GPUs.</summary>
    QualcommProprietary = 8,
    /// <summary>Proprietary driver for Arm GPUs.</summary>
    ArmProprietary = 9,
    /// <summary>Proprietary driver for Google Cloud/GGP platform.</summary>
    GgpProprietary = 11,
    /// <summary>Proprietary driver for Broadcom GPUs (used in some Raspberry Pi).</summary>
    BroadcomProprietary = 12,
    /// <summary>Proprietary driver for CoreAVI GPUs.</summary>
    CoreaviProprietary = 15,
    /// <summary>Proprietary driver for Juice GPUs.</summary>
    JuiceProprietary = 16,
    /// <summary>Proprietary driver for Verisilicon GPUs.</summary>
    VerisiliconProprietary = 17,
    /// <summary>Proprietary driver for Samsung GPUs.</summary>
    SamsungProprietary = 21,

    /// <summary>Driver translating Vulkan calls to Apple's Metal API.</summary>
    Moltenvk = 14,
    
    /// <summary>Open-source driver for AMD GPUs (non-Mesa).</summary>
    AmdOpenSource = 2,
}

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