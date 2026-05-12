using System.Reflection;
using System.Text;
using System.Text.Json;
//using MonoMod.RuntimeDetour;
using NapCatSharp.Mod.Core.ModTypes;
using NapCatSharp.Mod.Extensions;

namespace NapCatSharp.Mod.Core;

public static class ModConfigLoader
{
    internal readonly static string BasePath = System.IO.Path.Combine(AppContext.BaseDirectory, "configs", "mods");
    //private readonly static Dictionary<string, List<Hook>> configHooks = []; // Hook
    /// <summary>
    /// 获取模组文件夹路径
    /// </summary>
    /// <param name="modName"></param>
    /// <returns></returns>
    internal static string GetModConfigPath(string modName)
        => Path.Combine(BasePath, modName);

    internal static JsonSerializerOptions JOptions
    {
        get
        {
            return field ??= new JsonSerializerOptions()
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };
        }
        set => field = value;
    }

    internal static void ClearJsonCache()
    {
        JOptions = null!;
    }

    internal static void UndoWatch(string modName) // Hook
    {
        //var targetMod = configHooks.FirstOrDefault(kv => kv.Key.StartsWith(modName));
        //foreach (var item in targetMod.Value) {
        //    item.Dispose();
        //}
        //targetMod.Value.Clear();
        //configHooks.Remove(targetMod.Key);
    }

    internal static void LoadConfig(string modName, Assembly modAssembly)
    {
        if (!Directory.Exists(BasePath)) {
            Directory.CreateDirectory(BasePath);
        }
        var modConfigTypes = modAssembly.GetRealizeType<ModConfig>();

        foreach (var configType in modConfigTypes) {
            var cfPath = GetConfigFilePath(modName, configType);
            CreateFileIfNotExist(cfPath);
            var text = File.ReadAllText(cfPath, Encoding.UTF8);
            if(configType.FullName == null) {
                continue;
            }
            ModContext.ModConfigPropertySets[configType.FullName] = configType.GetPropertySets();
            if (string.IsNullOrWhiteSpace(text)) {
                var configObj = (ModConfig)Activator.CreateInstance(configType)!;
                ModLoader.logger.Info($"未初始化配置，进行初始化: {configObj.Name()}");
                configObj.ModName = modName;
                ModContext.AddConfig(modName,configObj);
                WriteFileToJson(cfPath, configObj, configType);
                //WatchProperties(configObj); // Hook
                continue;
            }

            var cfObj = (ModConfig)JsonSerializer.Deserialize(text, configType, JOptions)!;
            ModLoader.logger.Info($"配置已初始化，从配置反序列化: {configType.FullName}");
            //WatchProperties(cfObj); // Hook
            cfObj.ModName = modName;
            ModContext.AddConfig(modName, cfObj);
        }
    }

    /// <summary>
    /// 获取配置文件的路径
    /// </summary>
    public static string GetConfigFilePath(string modName, Type type)
    {
        var modCfPath = Path.Combine(BasePath, modName);
        return Path.Combine(modCfPath, type.Name + ".json");
    }

    /// <summary>
    /// 创建不存在的文件
    /// </summary>
    private static void CreateFileIfNotExist(string path)
    {
        if (!File.Exists(path)) {
            var dir = Path.GetDirectoryName(path);
            if(dir != null) {
                Directory.CreateDirectory(dir);
            }
            var fs = File.Create(path);
            fs.Dispose();
        }
    }

    private static void WriteFileToJson(string path, object obj, Type type)
    {
        var jsonText = JsonSerializer.Serialize(obj, type, JOptions);
        File.WriteAllText(path, jsonText);
    }

    #region Hook
    //private delegate void PropertySetDelegate(ModConfig config, object origValue);
    //private static void PropertySetHookMethod(PropertySetDelegate origMethod, ModConfig config, object origValue)
    //{
    //    origMethod.Invoke(config, origValue);
    //    config.SaveToFile();
    //}

    //private static void WatchProperties(ModConfig config)
    //{
    //    var validPropertys = config.GetType().GetProperties().Where(prop =>
    //        prop.GetCustomAttribute<JsonIgnoreAttribute>() == null || prop.GetSetMethod() != null);

    //    var cacheValidPropertys = Enumerable.Empty<PropertyInfo>();
    //    foreach (var p in validPropertys) {
    //        cacheValidPropertys = cacheValidPropertys.Concat(DepthGetPropertys(p));
    //    }

    //    validPropertys = cacheValidPropertys.Concat(validPropertys);

    //    if (configHooks.TryGetValue(config.Name(), out var hooks)) {
    //        hooks.ForEach(h => h.Dispose());
    //    }

    //    var propertySets = validPropertys.Select(f => f.GetSetMethod());
    //    hooks ??= [];
    //    foreach (var methodInfo in propertySets) {
    //        hooks.Add(new Hook(methodInfo!, PropertySetHookMethod));
    //        ModLoader.logger.Info($"监听配置属性: {config.Name()} {methodInfo!.Name}");
    //    }

    //    configHooks[config.Name()] = hooks;
    //}

    //private static IEnumerable<PropertyInfo> DepthGetPropertys(PropertyInfo info)
    //{
    //    var propertyType = info.PropertyType;
    //    if (!propertyType.IsPrimitive && propertyType != typeof(string) && !propertyType.IsAssignableTo(typeof(IEnumerable))) {
    //        var props = propertyType.GetProperties().Where(prop =>
    //            prop.GetCustomAttribute<JsonIgnoreAttribute>() == null || prop.GetSetMethod() != null);

    //        foreach (var p in props) {
    //            if (p.PropertyType.IsPrimitive) {
    //                yield return p;
    //            } else {
    //                foreach (var p2 in DepthGetPropertys(p)) {
    //                    yield return p2;
    //                }
    //            }
    //        }
    //    }
    //}
    #endregion

    //internal static void OverrideFileToJson(string modName, ModConfig config, Type type)
    //    => WriteFileToJson(Path.Combine(GetModConfigPath(modName), type.Name + ".json"), config, type);
}

/// <summary>
/// <see cref="ModConfigLoader"/> <br/>                                                                              <br/>
/// 程序启动时，从外部程序集加载<see cref="ModConfig"/>                                                              <br/>
///     - 如果在配置目录下( 程序根目录/configs/mods ) 存在与此配置的json文件则使用json文件反序列化为配置对象         <br/>
///     - 如果在配置目录下不存在此配置相关内容，则使用<see cref="Activator"/>创建，并序列化到本地                    <br/>
/// </summary>
//file static class Note{}