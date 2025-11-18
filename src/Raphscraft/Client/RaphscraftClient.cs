namespace Raphscraft.Client;

using System.Reflection;
using Raphscraft.Client.Render;
using Raphscraft.Client.Render.Vulkan;
using Raphscraft.Util;

using Raphscraft.Client.Render.Platform;

public class RaphscraftClient {
    private Window Window { get; }
    private RenderSystem RenderSystem { get; }
    private Logger Logger { get; }
    
    public RaphscraftClient() {
        Logger = Logger.Open();
        Logger.Info($"Booting up the Raph's Craft client - version {Assembly.GetExecutingAssembly().GetName().Version}");
        
        Window = new GlfwWindow() {
            Title = "Raph's Craft",
            Size = new(1280, 720),
            GraphicsApi = GraphicsApi.Vulkan
        };

        RenderSystem = new VkRenderSystem(Window);
        
        Window.Displayed = true;
    }

    public void Run() {
        while (!Window.ShouldClose) {
            Window.PollEvents();
            
            RenderSystem.Clear(Color.FromHsl(210, 1.0f, 0.6f));
            
            Window.Swap();
        }
    }
}