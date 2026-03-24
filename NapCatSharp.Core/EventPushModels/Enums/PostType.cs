using NapCatSharp.JsonConverter;
using System.Text.Json.Serialization;

namespace NapCatSharp.EventPushModels.Enums;

/// <summary>
/// 根类型
/// </summary>
[JsonConverter(typeof(PostTypeConverter))]
public enum PostType
{
    /// <summary> 消息 </summary>
    message,
    /// <summary> 通知 </summary>
    notice,
    /// <summary> 请求 </summary>
    request,
    /// <summary> 元事件 </summary>
    meta_event,
    /// <summary> 发送 </summary>
    message_sent
}