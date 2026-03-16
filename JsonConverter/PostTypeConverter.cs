using NapCatSharp.EventPushModels;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NapCatSharp.JsonConverter;

public class PostTypeConverter : JsonConverter<PostType>
{
    private static readonly string[] enumTypes;
    private static readonly PostType[] enumValues;
    static PostTypeConverter()
    {
        enumTypes = Enum.GetNames<PostType>();
        enumValues = Enum.GetValues<PostType>();
    }
    public override PostType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var valueStr = reader.GetString()
            ?? throw new JsonException($"将内容读取为{typeof(PostType).FullName}失败，字符串为空。{reader.Position}");
        if (enumTypes.Contains(valueStr)) {
            return enumValues.First(f => valueStr.Equals(f.ToString()));
        }
        throw new JsonException($"将内容读取为{typeof(PostType).FullName}失败，未找到给定类型。{valueStr}");
    }

    public override void Write(Utf8JsonWriter writer, PostType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
