using NapCatSharp.JsonConverter;
using System.Text.Json.Serialization;

namespace NapCatSharp.EventPushModels.Enums;

/// <summary>
/// 请求类型枚举
/// </summary>
[JsonConverter(typeof(RequestTypeConverter))]
public enum RequestType
{
    friend,
    group
}

public class RequestTypeConverter : EnumJsonConverter<RequestType>{}
