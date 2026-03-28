using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;

namespace NapCatSharp.Mod.Core;

public class ModContext : AssemblyLoadContext
{
    internal readonly static List<Mod/*WeakReference<NapCatSharp.Core.Mod>*/> Mods = [];
    /// <summary> 所以Mod的根目录 </summary>
    public readonly static string ModPath = Path.Combine(AppContext.BaseDirectory, "Mods");
    internal readonly static ConcurrentDictionary<string, Assembly/*WeakReference<Assembly>*/> ModAssemblys = [];
    internal readonly static ConcurrentDictionary<string, ModContext/*WeakReference<ModContext>*/> ModContexts = [];
    internal readonly string assemblyPath;
    public string? modName;
    public ModContext(string? name, bool isCollectible = true)
        : base(name, isCollectible)
    {
        modName = name;
        assemblyPath = "";
        if(name == null || modName == null) return;
        if (ModContexts.ContainsKey(name)) {
            throw new Exception($"已有同名模组, {name}");
        }
        //ModContexts[name] = new WeakReference<ModContext>(this);
        ModContexts[name] = this;
        assemblyPath =
            Path.Combine(ModPath, name, name + ".dll");

        Unloading += context => {
            ModAssemblys.TryRemove(modName, out _); // 移除Assembly引用
            ModContexts.TryRemove(modName, out _); // 移除context引用
            Mods.RemoveAll(f => f.ModName == modName);
            //Mods.RemoveAll(f => {
            //    if (f.TryGetTarget(out var target)) {
            //        return target.ModName == modName;
            //    }
            //    return false;
            //}); // 移除mod实例
        };
    }
    protected override Assembly? Load(AssemblyName assemblyName)
    {
        if(modName == null) return null;
        if(assemblyName.Name == null) return null;
        var assemblyPath = Path.Combine(ModPath, modName, assemblyName.Name + ".dll");

        if (!Path.Exists(assemblyPath)) {
            return null;
        }
        var assembly = LoadFromAssemblyPath(assemblyPath);
        if(modName == assemblyName.Name) {
            //ModAssemblys[modName] = new WeakReference<Assembly>(assembly);
            ModAssemblys[modName] = assembly;
        }
        return assembly;
    }

    protected override nint LoadUnmanagedDll(string unmanagedDllName)
    {
        if(modName == null) return nint.Zero;
        string modDir = Path.Combine(ModPath, modName);
        string nativeName = unmanagedDllName;

        if (!Path.HasExtension(nativeName)) {
            nativeName += RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ".dll" :
                          RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? ".so" :
                          ".dylib";
        }

        string nativePath = Path.Combine(modDir, nativeName);
        if (File.Exists(nativePath))
            return LoadUnmanagedDllFromPath(nativePath);

        return nint.Zero;
    }

    public static bool TryGetModAssembly(string modName, out Assembly?/*WeakReference<Assembly>?*/ assembly)
    {
        if(ModAssemblys.TryGetValue(modName, out var a)) {
            assembly = a;
            return true;
        }
        assembly = null;
        return false;
    }

    public static void UnLoadMod(string modName)
    {
        if (ModContexts.TryGetValue(modName, out var value)) {
            //if (value.TryGetTarget(out var target)) {
            //    target.Unload();
            //}
            value.Unload();
            GC.Collect(); // 不回收要等到被动回收，在那之前 文件仍然被引用
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
