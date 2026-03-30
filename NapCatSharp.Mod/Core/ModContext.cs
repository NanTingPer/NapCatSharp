using NapCatSharp.Mod.Core.ModTypes;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Xml.Linq;

namespace NapCatSharp.Mod.Core;

public class ModContext : AssemblyLoadContext
{
    internal readonly static List<ModTypes.Mod> Mods = [];
    /// <summary> 所有Mod的根目录 </summary>
    public readonly static string ModPath = Path.Combine(AppContext.BaseDirectory, "Mods");
    internal readonly static ConcurrentDictionary<string, Assembly> ModAssemblys = [];
    /// <summary>
    /// key是modName
    /// </summary>
    internal readonly static ConcurrentDictionary<string, ModContext> ModContexts = [];
    #region ModConfig
    /// <summary>
    /// key是modName, 值是此Mod的配置列表
    /// </summary>
    internal readonly static ConcurrentDictionary<string, List<ModConfig>> ModConfigs = [];
    /// <summary>
    /// key是类型的FullName
    /// </summary>
    internal readonly static Dictionary<string, PropertySets> ModConfigPropertySets = [];
    #endregion
    internal readonly string assemblyPath;
    public string? modName;
    private ModContext(string? name, bool isCollectible = true)
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

    public static bool TryGetModAssembly(string modName, out Assembly? assembly)
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
            ModAssemblys.TryRemove(modName, out _); // 移除Assembly引用
            ModContexts.TryRemove(modName, out _); // 移除context引用
            UnloadConfig(modName);
            UnloadModRef(modName);
            ModConfigLoader.ClearJsonCache();
            value.Unload();
            GC.Collect(); // 不回收要等到被动回收，在那之前 文件仍然被引用
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }

    private readonly static Lock createContextLock = new Lock();
    public static ModContext? GetOrCreate(string modName)
    {
        lock(createContextLock) {
            try {
                if (ModContexts.TryGetValue(modName, out var value)) {
                    return value;
                }
                var context = new ModContext(modName);
                ModContexts[modName] = context;
                return context;
            } catch(Exception e) {
                ModLoader.logger.Error($"上下文创建失败 {e.Message} {modName}");
                return null;
            }
        }
    }

    private readonly static Lock addConfigLock = new Lock();
    internal static void AddConfig(string modName, ModConfig config)
    {
        lock (addConfigLock) {
            try {
                if (ModConfigs.TryGetValue(modName, out var value)) {
                    value.Add(config);
                } else {
                    List<ModConfig> configs = [config];
                    ModConfigs[modName] = configs;
                }
            } catch{}
        }
    }

    #region Unload
    private static void UnloadConfig(string modName)
    {
        if (ModConfigs.Remove(modName, out var configs)) {
            foreach (var config in configs) {
                config.Unload();
                if(ModConfigPropertySets.Remove(config.Name(), out var value)) {
                    value.Clear();
                }
            }
        }
    }
    private static void UnloadModRef(string modName)
    {
        Mods.RemoveAll(f => {
            if (f.ModName == modName) {
                f.Unload();
                return true;
            }
            return false;
        });
    }
    #endregion
}

public class PropertySets
{
    public Dictionary<string, PropertyAccessor> PropertyMap { get; set; } = [];
    
    public void Add(string name, Action<object, object> set, Type proptype) =>
        PropertyMap[name] = new PropertyAccessor(){ Name = name, PropertyType = proptype, SetValue = set };
        PropertyMap[name] = new PropertyAccessor(){ Name = name,/* PropertyType = proptype, */SetValue = set };

    /// <summary>
    /// 尝试获取此属性的SetValue
    /// </summary>
    /// <param name="name">属性名</param>
    /// <param name="value">属性值</param>
    /// <returns></returns>
    public bool TryGetSet(string name, out PropertyAccessor value)
    {
        if (PropertyMap.TryGetValue(name, out value!)) {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 设置这个属性的值
    /// </summary>
    /// <param name="name">属性名</param>
    /// <param name="orig">原对象</param>
    /// <param name="newValue">新值</param>
    public void SetValue(string name, object orig, object newValue)
    {
        if(TryGetSet(name, out var value)) {
            value.SetValue?.Invoke(orig, newValue);
        }
    }

    public PropertyAccessor this[string name]
    {
        get
        {
            if (PropertyMap.TryGetValue(name, out PropertyAccessor? value)) {
                return value;
            }
            throw new KeyNotFoundException($"未找到{name}的属性设置器");
        }
        set
        {
            PropertyMap[name] = value;
        }
    }

    public void Clear()
    {
        foreach (var item in PropertyMap) {
            item.Value.SetValue = null!;
            //item.Value.PropertyType = null!;
        }
        PropertyMap.Clear();
    }
}

/// <summary>
/// 包含属性的Set
/// </summary>
public class PropertyAccessor
{
    /// <summary>
    /// 此属性的Set方法
    /// </summary>
    public required Action<object, object> SetValue { get; set; }
    /// <summary>
    /// 此属性的类型
    /// </summary>
    //public required Type PropertyType { get; set; }
    /// <summary>
    /// 此属性的名称
    /// </summary>
    public required string Name { get; set; }
}