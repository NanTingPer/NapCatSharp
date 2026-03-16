using NapCatSharp.RequestModels;
using NapCatSharp.RequestModels.OB11MessageModels;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NapCatSharp.JsonConverter;

public class OB11MessageModelFlagConver : JsonConverter<IOB11MessageModelFlag>
{
    public override IOB11MessageModelFlag? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // IOB11MessageModelFlag是脑袋 是整个消息的Root
        if(JsonDocument.TryParseValue(ref reader, out JsonDocument? document)) {
            var root = document.RootElement;
            if (!root.TryGetProperty(nameof(Text.Type).ToLower(),out var result)){
                throw new JsonException($"数据序列化为{typeof(IOB11MessageModelFlag).FullName}失败。未找到type属性");
            }
            var typeString = result.GetString();
            if (!OB11MessageType.typeNameMap.TryGetValue(typeString!, out var typeResult)) {
                throw new JsonException($"数据序列化为{typeof(IOB11MessageModelFlag).FullName}失败。位置类型: {typeString}");
            }
            if (OB11MessageType.messageTypeMap.TryGetValue(typeResult, out Type? msgType)) {
                return (IOB11MessageModelFlag)JsonSerializer.Deserialize(root, msgType)!;
            }
            return new Text(){ Data = new Text.OB11MessageText(){ Text = $"{typeResult}类型模型还未实现" } };
        }
        throw new JsonException($"数据序列化为{typeof(IOB11MessageModelFlag).FullName}失败。{reader.Position}");
    }

    public override void Write(Utf8JsonWriter writer, IOB11MessageModelFlag value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
