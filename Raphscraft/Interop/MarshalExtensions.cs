namespace Raphscraft.Interop;

using System.Runtime.InteropServices;

public static class MarshalExtensions {
    public unsafe static void FreeHGlobalArray(nint? arrayPtr, int elementCount) {
        if (arrayPtr == null || arrayPtr.Value == nint.Zero) return;
        
        var array = (nint*)arrayPtr.Value;
        for (var i = 0; i <  elementCount; i++)
            Marshal.FreeHGlobal(array[i]);
        Marshal.FreeHGlobal(arrayPtr.Value);
    }
}