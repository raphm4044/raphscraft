namespace Raphscraft.Client.Render.Vulkan;

using System.Runtime.InteropServices;
using Silk.NET.Vulkan;

public unsafe class VkGpu : Gpu {
    private PhysicalDevice VulkanPhysicalDevice { get; }

    public override string Name { get; }
    public override string Driver { get; }
    public override GpuKind Kind { get; }

    public VkGpu(Vk vk, PhysicalDevice physicalDevice) {
        VulkanPhysicalDevice = physicalDevice;
        
        PhysicalDeviceDriverProperties deviceDriverProperties = new();
        PhysicalDeviceProperties2 deviceProperties2 = new();
        deviceProperties2.AddNext(out deviceDriverProperties);
        
        vk.GetPhysicalDeviceProperties2(physicalDevice, &deviceProperties2);

        var deviceProperties = deviceProperties2.Properties;
        
        Name = Marshal.PtrToStringAnsi((nint)deviceProperties.DeviceName)!;
        Driver = Marshal.PtrToStringAnsi((nint)deviceDriverProperties.DriverInfo)!;
        Kind = deviceProperties.DeviceType switch {
            PhysicalDeviceType.IntegratedGpu => GpuKind.Integrated,
            PhysicalDeviceType.DiscreteGpu => GpuKind.Discrete,
            PhysicalDeviceType.Cpu => GpuKind.SoftwareRenderer,
            PhysicalDeviceType.VirtualGpu => GpuKind.Virtual,
            _ => GpuKind.SoftwareRenderer, // Just assume it's a software renderer, a.k.a. the worst kind of GPU (at least for real-time games)
        };
    }

    public static implicit operator PhysicalDevice(VkGpu gpu)
        => gpu.VulkanPhysicalDevice;
}