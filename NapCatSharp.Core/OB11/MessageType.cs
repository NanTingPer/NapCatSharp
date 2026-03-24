using NapCatSharp.JsonConverter;
using System.Text.Json.Serialization;

namespace NapCatSharp.OB11;

[JsonConverter(typeof(MessageTypeConverter))]
public enum MessageType
{
    @private,
    group
}

public class MessageTypeConverter : EnumJsonConverter<MessageType>{}
