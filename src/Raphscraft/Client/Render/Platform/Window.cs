namespace Raphscraft.Client.Render.Platform;

using Silk.NET.Core.Contexts;
using Silk.NET.Maths;

/// <summary>
/// A base class for a window.
/// </summary>
public abstract class Window : IDisposable {
    /// <summary>
    /// The graphics API.
    /// </summary>
    public virtual GraphicsApi GraphicsApi {
        get;
        set;
    } = GraphicsApi.OpenGl;

    /// <summary>
    /// A GL context for use with OpenGL / OpenGL ES.
    ///
    /// Will be null if GraphicsApi == Vulkan.
    /// </summary>
    public virtual IGLContext? Context { get; protected set; } 
    
    /// <summary>
    /// Should the window close.
    /// </summary>
    public abstract bool ShouldClose { get; }

    public virtual string Title { get; set; } = "Window";

    public virtual Vector2D<int> Size { get; set; } = new(800, 600);

    public virtual bool Displayed { get; set; }

    /// <summary>
    /// Return the instance extensions required to present on this window using Vulkan.
    /// </summary>
    /// <returns>A list of string, most of the time being ["VK_KHR_surface", "VK_KHR_(platform)_surface"]</returns>
    public abstract List<string> GetRequiredVulkanInstanceExtensions();
    
    /// <summary>
    /// Make OpenGL draw to this window.
    /// </summary>
    public abstract void MakeCurrent();
    
    /// <summary>
    /// Swap the framebuffers, causing a framebuffer to be displayed on-screen
    /// </summary>
    public abstract void Swap();
    
    /// <summary>
    /// Poll window events from the server
    /// </summary>
    public abstract void PollEvents();

    /// <summary>
    /// Dispose any native resources
    /// </summary>
    public abstract void Dispose();
}