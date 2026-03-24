using NapCatSharp.JsonConverter;
using System.Text.Json.Serialization;

namespace NapCatSharp.EventPushModels.Enums;

/// <summary>
/// 请求类型枚举
/// </summary>
[JsonConverter(typeof(RequestTypeConverter))]
public enum RequestType
{
    /// <summary> 私聊 </summary>
    friend,
    /// <summary> 群聊 </summary>
    group
}

/// <summary>
/// </summary>
public class RequestTypeConverter : EnumJsonConverter<RequestType>{}
