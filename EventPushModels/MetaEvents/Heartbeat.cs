using System.Text.Json.Serialization;

namespace NapCatSharp.EventPushModels.MetaEvents;

/// <summary>
/// 心跳
/// </summary>
public class Heartbeat : EventBaseModelG<MetaEventType>
{
    [JsonPropertyName("meta_event_type")]
    public MetaEventType MetaEventType { get; set; } = MetaEventType.heartbeat;

    [JsonPropertyName("status")]
    public required Status Statu { get; set; }

    public override MetaEventType GetEnumValue()
    {
        return MetaEventType.heartbeat;
    }

    public class Status
    {
        /// <summary>
        /// 当前 QQ 在线，null 表示无法查询到在线状态
        /// </summary>
        [JsonPropertyName("online")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Online { get; set; }

        /// <summary>
        /// 状态符合预期，意味着各模块正常运行、功能正常，且 QQ 在线
        /// </summary>
        [JsonPropertyName("good")]
        public bool Good { get; set; }
    }
}
