namespace Raphscraft.Client.Render.Vulkan;

using Raphscraft.Client.Render.Platform;
using Raphscraft.Interop;
using Silk.NET.Core;
using Silk.NET.Vulkan;
using System.Runtime.InteropServices;

public class VkInstanceManager : IDisposable {
    public Version32 ApiVersion { get; set; } = Vk.Version12;
    public Instance? Instance { get; private set; }

    private Vk _vk;
    private List<string> _requiredExtensions;
    private List<string> _requiredLayers;
    
    public VkInstanceManager(Vk vk, Window window) {
        _vk = vk;
        
        // Populate required extensions
        _requiredExtensions = window.GetRequiredVulkanInstanceExtensions();
        _requiredLayers = [];
    }
    
    public void AddRequiredExtension(string extension)                    => _requiredExtensions.Add(extension);
    public void AddRequiredExtensionRange(IEnumerable<string> extensions) => _requiredExtensions.AddRange(extensions);
    public void AddRequiredLayer(string layer)                            => _requiredLayers.Add(layer);
    public void AddRequiredLayerRange(IEnumerable<string> layers)         => _requiredLayers.AddRange(layers);

    public unsafe bool TryCreateInstance(out Result result) {
        byte** instanceExtensions = null;
        byte** instanceLayers = null;
        
        if (_requiredExtensions.Count > 0) {
            instanceExtensions = (byte**)Marshal.AllocHGlobal((nint)_requiredExtensions.Count * sizeof(byte*));
            for (var i = 0; i < _requiredExtensions.Count; i++) instanceExtensions[i] = (byte*)Marshal.StringToHGlobalAnsi(_requiredExtensions[i]);
        }
        if (_requiredLayers.Count > 0) {
            instanceLayers = (byte**)Marshal.AllocHGlobal((nint)_requiredLayers.Count * sizeof(byte*));
            for (var i = 0; i < _requiredLayers.Count; i++) instanceLayers[i] = (byte*)Marshal.StringToHGlobalAnsi(_requiredLayers[i]);
        }
        
        // ApplicationInfo has only 1 useful field: ApiVersion. No need to fill others.
        ApplicationInfo applicationInfo = new() { ApiVersion = ApiVersion };
        InstanceCreateInfo instanceCreateInfo = new() {
            EnabledExtensionCount = (uint)_requiredExtensions.Count,
            EnabledLayerCount = (uint)_requiredLayers.Count,
            PpEnabledExtensionNames = instanceExtensions,
            PpEnabledLayerNames = instanceLayers,
            PApplicationInfo = &applicationInfo
        };
        
        result = _vk.CreateInstance(instanceCreateInfo, null, out var newInstance);
        // Silk.NET will automatically load Vulkan instance APIs in the "Vk" instance after creating an instance.
        
        MarshalExtensions.FreeHGlobalArray((nint)instanceExtensions, _requiredExtensions.Count);
        MarshalExtensions.FreeHGlobalArray((nint)instanceLayers, _requiredLayers.Count);
        
        if (result != Result.Success) return false;
        Instance = newInstance;
        return true;
    }

    public static implicit operator Instance(VkInstanceManager instanceManager) {
        if (instanceManager.Instance != null) return instanceManager.Instance!.Value;
        
        var successful = instanceManager.TryCreateInstance(out var result);
        return successful
            ? instanceManager.Instance!.Value
            : throw new("Could not create instance: vkCreateInstance() returned " + result);

    }

    unsafe private void ReleaseUnmanagedResources() {
        if (Instance == null) return;
        _vk.DestroyInstance(Instance!.Value, null);
    }

    private void Dispose(bool disposing) {
        ReleaseUnmanagedResources();
        if (disposing) { _vk.Dispose(); }
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~VkInstanceManager() {
        Dispose(false);
    }
}