using NapCatSharp.JsonConverter;
using System.Text.Json.Serialization;

namespace NapCatSharp.EventPushModels.MessageEvents;

[JsonConverter(typeof(MessageTypeConverter))]
public enum MessageType
{
    @private,
    group
}

public class MessageTypeConverter : EnumJsonConverter<MessageType>{}
