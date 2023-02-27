using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace HqPocket.Extensions.Plugins;

public class CollectibleAssemblyLoadContext : AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver _resolver;
    public string AssemblyPath { get; }
    public CollectibleAssemblyLoadContext(string path)
        : base(isCollectible: true)
    {
        AssemblyPath = path;
        _resolver = new AssemblyDependencyResolver(AssemblyPath);
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        string? assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
        //return assemblyPath is not null ? LoadFromAssemblyPath(assemblyPath) : null;

        if (assemblyPath is null) return null;
        using FileStream fileStream = new(assemblyPath, FileMode.Open);
        return LoadFromStream(fileStream);
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        string? libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        return libraryPath is not null ? LoadUnmanagedDllFromPath(libraryPath) : IntPtr.Zero;
    }
}