using System.Reflection;
using System.Text;
using System.Text.Json;
using NapCatSharp.Mod.Core.ModTypes;
using NapCatSharp.Mod.Extensions;

namespace NapCatSharp.Mod.Core;

public static class ModConfigLoader
{
    internal readonly static string BasePath = System.IO.Path.Combine(AppContext.BaseDirectory, "configs", "mods");
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
                configObj.ModName = modName;
                ModContext.AddConfig(modName,configObj);
                WriteFileToJson(cfPath, configObj, configType);
                continue;
            }

            var cfObj = (ModConfig)JsonSerializer.Deserialize(text, configType, JOptions)!;
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

    //internal static void OverrideFileToJson(string modName, ModConfig config, Type type)
    //    => WriteFileToJson(Path.Combine(GetModConfigPath(modName), type.Name + ".json"), config, type);
}
