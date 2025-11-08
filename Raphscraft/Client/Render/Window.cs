namespace Raphscraft.Client.Render;

using Silk.NET.Core.Contexts;
using Silk.NET.GLFW;

/// <summary>
/// A GLFW abstraction
/// </summary>
public unsafe class Window : IDisposable {
    private readonly static Glfw Glfw = Glfw.GetApi();
    private static bool _glfwInitialized = false;
    private static int _instanceCount = 0;
    
    /// <summary>
    /// The GLFW window handle.
    /// </summary>
    public WindowHandle *Handle { get; private set; }
    public IGLContext Context { get; private set; } 
    public bool ShouldClose => Glfw.WindowShouldClose(Handle);
    
    public Window() {
        // Disable libdecor on non-GNOME desktops.
        if (!_glfwInitialized && Environment.GetEnvironmentVariable("DESKTOP_SESSION") != "gnome") {
            Glfw.GetVersion(out var _, out var minor, out var patch);
            if (minor > 3 || (minor == 3 && patch >= 9))
                Glfw.InitHint((InitHint)0x00053001 /*GLFW_WAYLAND_LIBDECOR*/, 0x00038002 /*GLFW_WAYLAND_DISABLE_LIBDECOR*/);
        }
        
        if (!_glfwInitialized && Glfw.Init())
            _glfwInitialized = true;

        Handle = Glfw.CreateWindow(800, 600, "Window", null, null);
        Glfw.MakeContextCurrent(Handle);

        Context = new GlfwContext(Glfw, Handle);
        _instanceCount++;
    }
    
    /// <summary>
    /// Swap the framebuffers, causing a framebuffer to be displayed on-screen
    /// </summary>
    public void Swap() => Glfw.SwapBuffers(Handle);
    
    /// <summary>
    /// Poll window events from the server
    /// </summary>
    public void PollEvents() => Glfw.PollEvents();
    
    /// <summary>
    /// Dispose any native resources, and terminate GLFW if no instance remains.
    /// </summary>
    public void Dispose() {
        Glfw.DestroyWindow(Handle);
        _instanceCount--;
        
        if (_instanceCount == 0)
            Glfw.Terminate();
        
        GC.SuppressFinalize(this);
    }
}