namespace Raphscraft.Client.Render;

using Raphscraft.Client.Render.Vertex;

/// <summary>
/// Base class for a render system.
/// </summary>
public abstract class RenderSystem {
    /// <summary>
    /// The graphics API this <see cref="RenderSystem"/> implements.
    /// </summary>
    public abstract GraphicsApi GraphicsApi { get; }

    /// <summary>
    /// Render a series of vertices & indices defined by a <see cref="VertexBuilder"/>.
    /// </summary>
    /// <param name="vertexBuilder">The vertex builder</param>
    public abstract void Draw(VertexBuilder vertexBuilder);
}