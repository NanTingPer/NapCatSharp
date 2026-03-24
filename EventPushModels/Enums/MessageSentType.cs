using NapCatSharp.JsonConverter;
using System.Text.Json.Serialization;

namespace NapCatSharp.EventPushModels.Enums;

/// <summary>
/// 自身发送的类型枚举
/// </summary>
[JsonConverter(typeof(MessageSentTypeConverter))]
public enum MessageSentType
{
    /// <summary>
    /// 自己
    /// </summary>
    self
}

public class MessageSentTypeConverter : EnumJsonConverter<MessageSentType>{}