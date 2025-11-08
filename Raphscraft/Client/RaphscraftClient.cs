namespace Raphscraft.Client;

using System.Drawing;
using Raphscraft.Client.Render;
using Silk.NET.OpenGL;

public class RaphscraftClient {
    Window _window;
    private GL gl;
    public RaphscraftClient() {
        _window = new();
        gl = GL.GetApi(_window.Context);
        gl.ClearColor(Color.BlueViolet);
    }

    public void Run() {
        while (!_window.ShouldClose) {
            _window.PollEvents();
            
            gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            _window.Swap();
        }
    }
}