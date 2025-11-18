namespace Raphscraft.Client.Render.Vulkan;

using System.Runtime.InteropServices;
using Raphscraft.Client.Render.Platform;
using Silk.NET.GLFW;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.EXT;

public unsafe sealed class VkRenderSystem : RenderSystem {
    public override GraphicsApi GraphicsApi => GraphicsApi.Vulkan;
    public override List<Gpu> AvailableGpus { get; }
    public override Gpu ActiveGpu { get; protected set; }

    private Vk _vk;

    //private bool _debug;
    private VkInstanceManager Instance { get; }
    private DebugUtilsMessengerEXT? DebugMessenger { get; }
    
    public VkRenderSystem(Window window, bool debug = false) : base(window) {
        if (!Glfw.GetApi().VulkanSupported()) throw new("No Vulkan loader or ICD was found on your PC.");

        Instance = new(_vk = Vk.GetApi(), window);
        if (debug) {
            Instance.AddRequiredLayer(VkValidationLayer.Name);
            Instance.AddRequiredExtension(ExtDebugUtils.ExtensionName);
            DebugMessenger = VkValidationLayer.Setup(_vk, Instance);
        }
        

        AvailableGpus = VkUtil.EnumerateGpus(_vk, Instance).Select(Gpu (vkGpu) => vkGpu).ToList();
        ActiveGpu = AvailableGpus[0];
        
        Logger.Info("Available GPUs:");
        AvailableGpus.ForEach((gpu) => {
            var score = gpu.GetScore();
            Logger.Info($" - {gpu.Name} ({gpu.Kind}, drove by {gpu.Driver}) - Score: " + score);
            if (ActiveGpu.GetScore() < score)
                ActiveGpu = (VkGpu)gpu;
        });
        Logger.Info($"Selected GPU: {ActiveGpu.Name} ({ActiveGpu.Kind}, drove by {ActiveGpu.Driver})");
    }

    public override void Clear(Color color) => throw new NotImplementedException("Vulkan renderer isn't implemented");

    protected override void ReleaseUnmanagedResources() {
        if (DebugMessenger != null)
            VkValidationLayer.DestroyMessenger(_vk, Instance, DebugMessenger.Value);
        
        _vk.DestroyInstance(Instance, null);
    }
}