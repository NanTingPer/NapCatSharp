using NapCatSharp.JsonConverter;
using NapCatSharp.OB11;
using System.Text.Json.Serialization;

namespace NapCatSharp.EventPushModels.MessageEvents;

public class GroupMessage : EventBaseModelG<MessageType>
{
    [JsonPropertyName("message_type")]
    public MessageType MessageType { get; set; }

    /// <summary>
    /// 消息子类型，正常消息是 normal，匿名消息是 anonymous，系统提示（如「管理员已禁止群内匿名聊天」）是 notice
    /// </summary>
    [JsonPropertyName("sub_type")]
    public SubTypeEnum SubType { get; set; }

    [JsonPropertyName("message_id")]
    public LongId MessageId { get; set; }

    [JsonPropertyName("group_id")]
    public LongId GroupId { get; set; }
    [JsonPropertyName("user_id")]
    public LongId UserId { get; set; }

    /// <summary>
    /// 匿名信息，如果不是匿名消息则为 null
    /// </summary>
    [JsonPropertyName("anonymous")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AnonymousData? Anonymous { get; set; }

    /// <summary>
    /// 消息内容
    /// </summary>
    [JsonPropertyName("message")]
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
    public GroupSender? Sender { get; set; }

    public override MessageType GetEnumValue()
    {
        return MessageType.group;
    }

    [JsonConverter(typeof(SubTypeEnumConverter))]
    public enum SubTypeEnum
    {
        /// <summary>
        /// 正常消息
        /// </summary>
        normal,
        /// <summary>
        /// 匿名消息
        /// </summary>
        anonymous,
        /// <summary>
        /// 系统提示 如「管理员已禁止群内匿名聊天」
        /// </summary>
        notice
    }

    public class SubTypeEnumConverter : EnumJsonConverter<SubTypeEnum>{}

    public class AnonymousData
    {
        /// <summary>
        /// 匿名用户 ID
        /// </summary>
        [JsonPropertyName("id")]
        public LongId Id { get; set; }

        /// <summary>
        /// 匿名用户名称
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 匿名用户 flag，在调用禁言 API 时需要传入
        /// </summary>
        [JsonPropertyName("flag")]
        public string Flag { get; set; } = string.Empty;
    }
}
