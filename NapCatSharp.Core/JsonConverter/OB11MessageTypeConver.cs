using NapCatSharp.OB11;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NapCatSharp.JsonConverter;

public class OB11MessageTypeConver : JsonConverter<OB11MessageType>
{
    public override OB11MessageType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try {
            if (reader.TokenType == JsonTokenType.String) {
                var string_ = reader.GetString();
                if(OB11MessageType.typeNameMap.TryGetValue(string_ ?? "", out OB11MessageType result)) {
                    return result;
                }
            }
        } catch { }

        throw new JsonException($"给定的Json属性无法序列化读取为{typeof(OB11MessageType).FullName}. 在字典中未找到相关类型。");
    }

    public override void Write(Utf8JsonWriter writer, OB11MessageType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.TypeName);
    }

}
