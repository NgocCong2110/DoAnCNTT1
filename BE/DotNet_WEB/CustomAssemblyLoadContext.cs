using System;
using System.Runtime.Loader;
using System.Runtime.InteropServices;

public class CustomAssemblyLoadContext : AssemblyLoadContext
{
    public IntPtr LoadUnmanagedLibrary(string absolutePath)
    {
        return LoadUnmanagedDll(absolutePath);
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllPath)
    {
        return NativeLibrary.Load(unmanagedDllPath);
    }

    protected override System.Reflection.Assembly Load(System.Reflection.AssemblyName assemblyName) => null;
}
