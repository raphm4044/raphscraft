namespace Raphscraft.Client.Render.Vulkan;

using System.ComponentModel;
using System.Runtime.InteropServices;
using Silk.NET.Core;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.EXT;

public unsafe static class VkValidationLayer {
    public const string Name = "VK_LAYER_KHRONOS_validation";

    private static uint Callback(
        DebugUtilsMessageSeverityFlagsEXT severity, 
        DebugUtilsMessageTypeFlagsEXT type, 
        DebugUtilsMessengerCallbackDataEXT *callbackData, 
        void *userdata) {

        Console.WriteLine("VkValidation: " + Marshal.PtrToStringUTF8((nint)callbackData->PMessage));
        
        return 0;
    }
    
    public static DebugUtilsMessengerEXT Setup(Vk vk, Instance instance) {
        if (!vk.TryGetInstanceExtension<ExtDebugUtils>(instance, out var dbgUtilsExt))
            throw new("Could not obtain the DebugUtils extension.");

        DebugUtilsMessengerCreateInfoEXT messengerCreateInfo = new() {
            MessageSeverity = 
                DebugUtilsMessageSeverityFlagsEXT.ErrorBitExt | 
                DebugUtilsMessageSeverityFlagsEXT.WarningBitExt |
                DebugUtilsMessageSeverityFlagsEXT.VerboseBitExt,
            MessageType = 
                DebugUtilsMessageTypeFlagsEXT.ValidationBitExt | 
                DebugUtilsMessageTypeFlagsEXT.PerformanceBitExt | 
                DebugUtilsMessageTypeFlagsEXT.GeneralBitExt,
            PUserData = null,
            PfnUserCallback = new(Callback),
        };

        dbgUtilsExt.CreateDebugUtilsMessenger(instance, &messengerCreateInfo, null, out var messenger);
        return messenger;
    }

    public static void DestroyMessenger(Vk vk, Instance instance, DebugUtilsMessengerEXT messenger) {
        if (!vk.TryGetInstanceExtension<ExtDebugUtils>(instance, out var dbgUtilsExt))
            throw new("Could not obtain the DebugUtils extension.");

        dbgUtilsExt.DestroyDebugUtilsMessenger(instance, messenger, null);
    }
}