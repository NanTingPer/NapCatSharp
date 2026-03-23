using NapCatSharp.EventPushModels;
using NapCatSharp.EventPushModels.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NapCatSharp.JsonConverter;

public abstract class EnumJsonConverter<TEnum> : JsonConverter<TEnum>
    where TEnum : struct, Enum
{
    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var typeStr = reader.GetString();
        if (typeStr == null || !EnumExtension<TEnum>.Contains(typeStr)) {
            throw new JsonException($"无法将 {typeStr ?? "null"} 转换为{typeof(MetaEventType).FullName}  {reader.Position}");
        }
        return EnumExtension<TEnum>.GetValue(typeStr) ??
            throw new JsonException($"在 {typeof(MetaEventType).FullName} 中为找到定义 {typeStr ?? "null"}   {reader.Position}");
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
