using NapCatSharp.JsonConverter;
using System.Text.Json.Serialization;

namespace NapCatSharp.EventPushModels;

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
    meta_event
}