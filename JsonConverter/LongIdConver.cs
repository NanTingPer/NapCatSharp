using NapCatSharp.RequestModels;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NapCatSharp.JsonConverter;

public class LongIdConverter : JsonConverter<LongId>
{
    public override LongId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try {
            if (reader.TokenType == JsonTokenType.String) {
                var string_ = reader.GetString();
                if (long.TryParse(string_, out long result)) {
                    return new LongId() { String = string_, Long = result };
                }
            }

            if(reader.TokenType == JsonTokenType.Number) {
                if(reader.TryGetInt64(out long result)){
                    return new LongId() { String = result.ToString(), Long = result };
                }
            }

        } catch{}
        
        throw new JsonException($"给定的Json属性无法序列化读取为{typeof(LongId).FullName}.");
    }

    public override void Write(Utf8JsonWriter writer, LongId value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Long);
    }
}
