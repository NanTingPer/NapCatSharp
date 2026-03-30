using NapCatSharp.Mod.Core.ModTypes;

namespace NapCatSharp.Mod.Core;

public static class ModConfigManager
{
    /// <summary>
    /// 获取给定配置
    /// </summary>
    internal static T? GetConfig<T>()
        where T : ModConfig
    {
        var config = ModContext.ModConfigs.Values.SelectMany(f => f).FirstOrDefault(f => f.Name == typeof(T).FullName);
        if (config != null) {
            return (T)config;
        }
        return null;
    }
}