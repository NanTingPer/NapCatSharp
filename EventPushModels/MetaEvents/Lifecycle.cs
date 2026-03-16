using NapCatSharp.JsonConverter;
using System.Text.Json.Serialization;

namespace NapCatSharp.EventPushModels.MetaEvents;

/// <summary>
/// 生命周期事件
/// </summary>
public class Lifecycle : EventBaseModelG<MetaEventType>
{
    [JsonPropertyName("meta_event_type")]
    public MetaEventType MetaEventType { get; set; } = MetaEventType.lifecycle;

    [JsonPropertyName("sub_type")]
    public SubType Type { get; set; }

    public override MetaEventType GetEnumValue()
    {
        return MetaEventType.lifecycle;
    }

    [JsonConverter(typeof(SubTypeConverter))]
    public enum SubType
    {
        /// <summary>
        /// OneBot 启用
        /// </summary>
        enable,
        /// <summary>
        /// OneBot 停用
        /// </summary>
        disable,
        /// <summary>
        /// WebSocket 连接成功
        /// </summary>
        connect
    }

    public class SubTypeConverter : EnumJsonConverter<SubType>
    {

    }
}