using NapCatSharp.JsonConverter;
using NapCatSharp.OB11;
using System.Text.Json.Serialization;

namespace NapCatSharp.EventPushModels.MessageEvents;

/// <summary>
/// <see href="https://github.com/botuniverse/onebot-11/blob/d4456ee706f9ada9c2dfde56a2bcfc69752600e4/event/message.md"/>
/// </summary>
public class PrivateMessage : EventBaseModelG<MessageType>
{
    //message_type
    /// <summary>
    /// 消息类型
    /// </summary>
    [JsonPropertyName("sub_type")]
    public MessageType MessageType { get; set; }

    /// <summary>
    /// 消息子类型
    /// </summary>
    [JsonPropertyName("sub_type")]
    public SubTypeEnum SubType { get; set; }

    /// <summary>
    /// 消息 ID
    /// </summary>
    [JsonPropertyName("message_id")]
    public LongId MessageId { get; set; }

    /// <summary>
    /// 发送者 QQ 号
    /// </summary>
    [JsonPropertyName("user_id")]
    public LongId UserId { get; set; }

    /// <summary>
    /// 消息内容
    /// </summary>
    [JsonPropertyName("message")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<IOB11MessageModelFlag> Message { get; set; } = [];

    /// <summary>
    /// 原始消息内容
    /// </summary>
    [JsonPropertyName("raw_message")]
    public string RawMessage { get; set; } = string.Empty;

    /// <summary>
    /// 字体
    /// </summary>
    [JsonPropertyName("font")]
    public LongId Font { get; set; }

    /// <summary>
    /// 发送人信息
    /// </summary>
    [JsonPropertyName("sender")]
    public required PrivateSender Sender { get; set; }

    public override MessageType GetEnumValue()
    {
        return MessageType.@private;
    }

    [JsonConverter(typeof(SubTypeEnumConverter))]
    public enum SubTypeEnum
    {
        /// <summary>
        /// 好友
        /// </summary>
        friend,
        /// <summary>
        /// 群临时会话
        /// </summary>
        group,
        other
    }

    public class SubTypeEnumConverter : EnumJsonConverter<SubTypeEnum>{}
}
