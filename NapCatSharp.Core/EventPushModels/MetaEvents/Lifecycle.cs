using NapCatSharp.EventPushModels.Enums;
using NapCatSharp.JsonConverter;
using System.Text.Json.Serialization;

namespace NapCatSharp.EventPushModels.MetaEvents;

/// <summary>
/// 生命周期事件
/// </summary>
public class Lifecycle : EventBaseModelG<MetaType>
{
    [JsonPropertyName("meta_event_type")]
    public MetaType MetaEventType { get; set; } = MetaType.lifecycle;

    [JsonPropertyName("sub_type")]
    public SubType Type { get; set; }

    public override MetaType GetEnumValue()
    {
        return MetaType.lifecycle;
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