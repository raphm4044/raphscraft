namespace Raphscraft.Client.Render;

using Raphscraft.Client.Render.Platform;
using Raphscraft.Util;

/// <summary>
/// Base class for a RenderSystem implementation.
/// </summary>
public abstract class RenderSystem : IDisposable {
    /// <summary>
    /// The graphics API this <see cref="RenderSystem"/> implements.
    /// </summary>
    public abstract GraphicsApi GraphicsApi { get; }

    /// <summary>
    /// All available GPUs.
    /// </summary>
    public abstract List<Gpu> AvailableGpus { get; }
    
    /// <summary>
    /// The GPU currently in use.
    /// </summary>
    public abstract Gpu ActiveGpu { get; protected set;  }
    
    /// <summary>
    /// The <see cref="Logger"/> instance for logging RenderSystem messages.
    /// </summary>
    protected Logger Logger { get; }
    
    public RenderSystem(Window window) {
        _window = window;
        window.GraphicsApi = GraphicsApi;
        Logger = Logger.Open("RenderSystem");
        Logger.Info("Using " + GetType().Name);
    }

    /// <summary>
    /// Clear the depth & color buffers.
    ///
    /// This method will be deprecated when corresponding abstractions are created.
    /// </summary>
    /// <param name="color">The clear color for the color buffer</param>
    public abstract void Clear(Color color);
    
    protected abstract void ReleaseUnmanagedResources();

    private void Dispose(bool disposing) {
        ReleaseUnmanagedResources();
        if (disposing) { _window.Dispose(); }
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~RenderSystem() {
        Dispose(false);
    }
    
    protected Window _window;
}