using System.Text.Json;
using System.Text.Json.Serialization;

namespace NapCatSharp.RequestModels.JsonConverter;

public class StringIdConverter : JsonConverter<StringId>
{
    public override StringId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try {
            if (reader.TokenType == JsonTokenType.String) {
                var string_ = reader.GetString();
                if (long.TryParse(string_, out long result)) {
                    return new StringId() { String = string_, Long = result };
                }
            }

            if (reader.TokenType == JsonTokenType.Number) {
                if (reader.TryGetInt64(out long result)) {
                    return new StringId() { String = result.ToString(), Long = result };
                }
            }

        } catch { }

        throw new JsonException($"给定的Json属性无法序列化读取为{typeof(StringId).FullName}.");
    }

    public override void Write(Utf8JsonWriter writer, StringId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.String);
    }

}
