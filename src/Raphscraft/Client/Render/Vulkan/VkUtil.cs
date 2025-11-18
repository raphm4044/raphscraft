namespace Raphscraft.Client.Render.Vulkan;

using System.Runtime.InteropServices;
using Silk.NET.Vulkan;

public static class VkUtil {
    public unsafe static bool LayerSupported(Vk vk, string name) {
        Span<uint> layerCount = stackalloc uint[1];
        vk.EnumerateInstanceLayerProperties(layerCount, (LayerProperties*)null);

        Span<LayerProperties> layers = stackalloc LayerProperties[(int)layerCount[0]];
        vk.EnumerateInstanceLayerProperties(layerCount, layers);

        foreach (var layer in layers) {
            if (name == Marshal.PtrToStringAnsi((nint)layer.LayerName)) 
                return true;
        }

        return false;
    }

    public unsafe static List<VkGpu> EnumerateGpus(Vk vk, Instance instance) {
        uint gpuCount;
        vk.EnumeratePhysicalDevices(instance, &gpuCount, null);
        Span<PhysicalDevice> physicalDevices = stackalloc PhysicalDevice[(int)gpuCount];
        vk.EnumeratePhysicalDevices(instance, &gpuCount, physicalDevices);
        
        return physicalDevices
            .ToArray()
            .Select(gpu => new VkGpu(vk, gpu))
            .ToList();
    }
}