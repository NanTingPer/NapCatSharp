using System.Text.Json.Serialization;

namespace NapCatSharp.EventPushModels;

public abstract class EventBaseModel
{
    [JsonPropertyName("time")]
    public long Time { get; set; }

    /// <summary> 收到事件推送的qq号，也就是机器人qq号 </summary>
    [JsonPropertyName("self_id")]
    public long SelfId { get; set; }

    /// <summary> 事件类型 </summary>
    [JsonPropertyName("post_type")]
    public PostType EventType { get; set; }

    [JsonIgnore]
    public bool IsMessage => EventType == PostType.message;
    [JsonIgnore]
    public bool IsRequest => EventType == PostType.request; 
    [JsonIgnore]
    public bool IsMetaEvent => EventType == PostType.meta_event;
    [JsonIgnore]
    public bool IsNotice => EventType == PostType.notice;
}
