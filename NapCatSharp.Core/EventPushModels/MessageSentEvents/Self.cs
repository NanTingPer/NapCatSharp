using NapCatSharp.EventPushModels.Enums;
using NapCatSharp.JsonConverter;
using NapCatSharp.OB11;
using System.Text.Json.Serialization;

namespace NapCatSharp.EventPushModels.MessageSentEvents;

public class Self : EventBaseModelG<MessageSentType>
{
    [JsonPropertyName("message_type")]
    public MessageType MessageType { get; set; }

    [JsonPropertyName("sub_type")]
    public SubTypeEnum SubType { get; set; }

    [JsonPropertyName("message_id")]
    public LongId MessageId { get; set; }

    [JsonPropertyName("message_seq")]
    public LongId MessageSeq { get; set; }

    [JsonPropertyName("real_id")]
    public LongId RealId { get; set; }

    [JsonPropertyName("real_seq")]
    public string RealSeq { get; set; } = string.Empty;

    [JsonPropertyName("sender")]
    public required SelfSender Sender { get; set; }

    [JsonPropertyName("raw_message")]
    public string RawMessage { get; set; } = string.Empty;

    [JsonPropertyName("font")]
    public LongId Font { get; set; }

    [JsonPropertyName("message")]
    public List<IOB11MessageModelFlag> Message { get; set; } = [];

    [JsonPropertyName("message_format")]
    public string MessageFormat { get; set; } = "array";

    [JsonPropertyName("target_id")]
    public LongId TargetId { get; set; }

    [JsonPropertyName("message_sent_type")]
    public MessageSentType MessageSentType { get; set; } = MessageSentType.self;

    public override MessageSentType GetEnumValue()
    {
        return MessageSentType.self;
    }

    [JsonConverter(typeof(SubTypeEnumConverter))]
    public enum SubTypeEnum
    {
        friend,
        group,
        other
    }

    public class SubTypeEnumConverter : EnumJsonConverter<SubTypeEnum>{}
}

public class SelfSender
{
    [JsonPropertyName("user_id")]
    public LongId UserId { get; set; }

    [JsonPropertyName("nickname")]
    public string NickName { get; set; } = string.Empty;

    [JsonPropertyName("card")]
    public string Card { get; set; } = string.Empty;
}