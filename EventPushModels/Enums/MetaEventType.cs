using NapCatSharp.JsonConverter;
using System.Text.Json.Serialization;

namespace NapCatSharp.EventPushModels.Enums;

/// <summary>
/// 元事件
/// <br/> OneBot 自己产生一类事件
/// </summary>
[JsonConverter(typeof(MetaEventTypeConverter))]
public enum MetaEventType
{
    /// <summary>
    /// 生命周期
    /// </summary>
    lifecycle,
    /// <summary>
    /// 心跳
    /// </summary>
    heartbeat,
}

public class MetaEventTypeConverter : EnumJsonConverter<MetaEventType>
{
}
