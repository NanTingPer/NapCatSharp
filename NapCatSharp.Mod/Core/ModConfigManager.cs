using NapCatSharp.Mod.Core.ModTypes;
using System.Text.Json.Nodes;

namespace NapCatSharp.Mod.Core;

public static class ModConfigManager
{
    /// <summary>
    /// 获取给定配置
    /// </summary>
    internal static T? GetConfig<T>()
        where T : ModConfig
    {
        var config = ModContext.ModConfigs.Values.SelectMany(f => f).FirstOrDefault(f => f.Name() == typeof(T).FullName);
        if (config != null) {
            return (T)config;
        }
        return null;
    }

    /// <summary>
    /// 获取给定模组的配置
    /// </summary>
    public static List<ModConfig> GetConfigs(string modName)
    {
        return ModContext.ModConfigs[modName];
    }

    /// <summary>
    /// 获取全部配置
    /// </summary>
    public static List<(string modName, List<ModConfig>)> GetAllConfigs()
    {
        return [.. ModContext.ModConfigs.Select(f => (f.Key, f.Value))];
    }
    
    /// <summary>
    /// 更新配置
    /// </summary>
    /// <param name="modName"> 配置所属的模组名称 </param>
    /// <param name="configName"> 配置名称 </param>
    /// <param name="configValue"> 新的配置值 </param>
    public static void UpdateConfig(string modName, string configName, JsonObject configValue)
    {
        if (!ModContext.ModConfigs.TryGetValue(modName, out var configs)) {
            return;
        }
        var tarconfigs = configs.FirstOrDefault(f => f.Name() == configName);
        tarconfigs?.CompleteCover(configValue);
    }
}