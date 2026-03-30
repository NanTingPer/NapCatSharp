using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace NapCatSharp.Mod.Core.ModTypes;

public class ModConfig : ModType
{
    [JsonIgnore]
    internal string Name => GetType().FullName ?? "动态类型";
    internal string ModName { get; set; } = string.Empty;

    /// <summary>
    /// 将对象属性值全部覆盖为传入的<see cref="JsonObject"/> 
    /// </summary>
    internal void CompleteCover(JsonObject jobject)
    {
        if (!ModContext.ModConfigPropertySets.TryGetValue(Name, out var propSet)){
            ModLoader.logger.Warning($"未找到{Name}的属性设置器，请确保此配置被注册");
            return;
        }
        var props = GetType().GetProperties();
        foreach (var keyValue in jobject) {
            var tarProp = props.FirstOrDefault(p => p.Name.Equals(keyValue.Key, StringComparison.OrdinalIgnoreCase));
            if (tarProp == null) continue;
            object? newValue;
            try {
                newValue = JsonSerializer.Deserialize(keyValue.Value, tarProp.PropertyType);
            } catch(Exception) {
                ModLoader.logger.Warning($"将内容{keyValue.Value?.ToJsonString() ?? "null"}序列化为 {tarProp.PropertyType.FullName}失败");
                continue;
            }
            if (newValue == null) return;
            if (propSet.TryGetSet(keyValue.Key, out var set)) {
                ModLoader.logger.Warning($"未找到{Name} {keyValue.Key}的属性设置器，请确保未排除此项");
                continue;
            }
            set.SetValue?.Invoke(this, newValue);
        }
    }
}