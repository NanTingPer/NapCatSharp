using System.Text.Json.Serialization;
using NapCatSharp;
using NapCatSharp.RequestModels;

namespace NapCatSharp.RequestModels.GroupInterfaces;

/// <summary>移出精华消息</summary>
public class DeleteEssenceMsg : RequestModelBase
{
    /// <summary>消息ID</summary>
    [JsonPropertyName("message_id")]
    public long MessageId { get; set; }

    /// <summary>消息序号</summary>
    [JsonPropertyName("msg_seq")]
    public string? MsgSeq { get; set; }

    /// <summary>消息随机数</summary>
    [JsonPropertyName("msg_random")]
    public string? MsgRandom { get; set; }

    /// <summary>群号</summary>
    [JsonPropertyName("group_id")]
    public LongId? GroupId { get; set; }

    public override string GetEndpoint()
    {
        return "/delete_essence_msg";
    }
}
