namespace Raphscraft.Client.Render.Platform;

using System.Runtime.InteropServices;
using Silk.NET.Core.Contexts;
using Silk.NET.GLFW;
using Silk.NET.Maths;

/// <summary>
/// A <see cref="Window"/> implementation using Graphics Library FrameWork.
/// </summary>
public unsafe sealed class GlfwWindow : Window {
    private readonly static Glfw Glfw = Glfw.GetApi();
    private static bool GlfwInitialized { get; set; }
    private static int InstanceCount { get; set; }

    /// <summary>
    /// The graphics API.
    /// </summary>
    public override GraphicsApi GraphicsApi {
        get;
        set {
            field = value;
            Recreate();
        }
    } = GraphicsApi.OpenGl;

    /// <summary>
    /// The GLFW window handle.
    /// </summary>
    public WindowHandle* Handle { get; private set; } = null;
    public override IGLContext? Context { get; protected set; } 
    
    /// <summary>
    /// Should the window close.
    /// </summary>
    public override bool ShouldClose => Glfw.WindowShouldClose(Handle);

    public override string Title {
        get;
        set {
            field = value;
            Glfw.SetWindowTitle(Handle, value);
        }
    } = "Window";

    public override Vector2D<int> Size {
        get;
        set {
            field = value;
            Glfw.SetWindowSize(Handle, value.X, value.Y);
        }
    } = new(800, 600);

    public override bool Displayed {
        get => Glfw.GetWindowAttrib(Handle, WindowAttributeGetter.Visible);
        set { if (value) Glfw.ShowWindow(Handle); else Glfw.HideWindow(Handle); }
    }
    
    public GlfwWindow() {
        // Disable libdecor on non-GNOME Wayland desktops
        if (!GlfwInitialized && !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WAYLAND_DISPLAY")) && Environment.GetEnvironmentVariable("DESKTOP_SESSION") != "gnome") {
            Glfw.GetVersion(out var _, out var minor, out var patch);
            if (minor > 3 || (minor == 3 && patch >= 9))
                Glfw.InitHint((InitHint)0x00053001 /*GLFW_WAYLAND_LIBDECOR*/, 0x00038002 /*GLFW_WAYLAND_DISABLE_LIBDECOR*/);
        }
        
        if (!GlfwInitialized && Glfw.Init())
            GlfwInitialized = true;

        Recreate();
        InstanceCount++;
    }

    /// <summary>
    /// Recreate the window handle
    /// </summary>
    /// <exception cref="Exception"></exception>
    public void Recreate() {
        if (Handle != null) {
            Context?.Dispose();
            Glfw.DestroyWindow(Handle);
        }

        switch (GraphicsApi) {
            case GraphicsApi.Vulkan:   Glfw.WindowHint(WindowHintClientApi.ClientApi, ClientApi.NoApi); break;
            case GraphicsApi.OpenGl:   
                Glfw.WindowHint(WindowHintClientApi.ClientApi, ClientApi.OpenGL); 
                Glfw.WindowHint(WindowHintInt.ContextVersionMajor, 4);
                Glfw.WindowHint(WindowHintInt.ContextVersionMinor, 6);
                break;
            case GraphicsApi.OpenGles: Glfw.WindowHint(WindowHintClientApi.ClientApi, ClientApi.OpenGLES); break;
            case GraphicsApi.None:     throw new("Invalid graphics API: None. You should use GraphicsApi.Vulkan if you want to use Vulkan.");
            default:                   throw new($"Invalid graphics API: {GraphicsApi}");
        }
        
        Glfw.WindowHint(WindowHintBool.Visible, false);
        Handle = Glfw.CreateWindow(Size.X, Size.Y, Title, null, null);
        if (Handle == null) {
            var code = Glfw.GetError(out var desc);
            throw new("Could not create the window handle: " + Marshal.PtrToStringUTF8((nint)desc));
        }
        Context = new GlfwContext(Glfw, Handle);
    }

    /// <summary>
    /// Return the instance extensions required to present on this window using Vulkan.
    /// </summary>
    /// <returns>A list of string, most of the time being ["VK_KHR_surface", "VK_KHR_(platform)_surface"]</returns>
    public override List<string> GetRequiredVulkanInstanceExtensions() {
        var ret = (List<string>)[];
        
        var glfwInstanceExtensions = Glfw.GetApi().GetRequiredInstanceExtensions(out var glfwInstanceExtCount);
        Glfw.GetApi().GetError(out var errorString);
        if (glfwInstanceExtensions == null) 
            throw new("Could not obtain instance extensions: " + Marshal.PtrToStringAnsi((nint)errorString));

        for (var i = 0; i < (int)glfwInstanceExtCount; i++)
            ret.Add(Marshal.PtrToStringAnsi((nint)glfwInstanceExtensions[i])!);

        return ret;
    }

    /// <summary>
    /// Make OpenGL draw to this window.
    /// </summary>
    public override void MakeCurrent() => Glfw.MakeContextCurrent(Handle);
    
    /// <summary>
    /// Swap the framebuffers, causing a framebuffer to be displayed on-screen
    /// </summary>
    public override void Swap() => Glfw.SwapBuffers(Handle);
    
    /// <summary>
    /// Poll window events from the server
    /// </summary>
    public override void PollEvents() => Glfw.PollEvents();
    
    /// <summary>
    /// Dispose any native resources, and terminate GLFW if no instance remains.
    /// </summary>
    public override void Dispose() {
        Context?.Dispose();
        Glfw.DestroyWindow(Handle);
        InstanceCount--;
        
        if (InstanceCount == 0)
            Glfw.Terminate();
    }
}