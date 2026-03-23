using NapCatSharp.JsonConverter;
using System.Text.Json.Serialization;

namespace NapCatSharp.OB11;

/// <summary>群系统消息通知</summary>
public class OB11Notify
{
    /// <summary>请求ID</summary>
    [JsonPropertyName("request_id")]
    public LongId RequestId { get; set; }

    /// <summary>邀请者QQ</summary>
    [JsonPropertyName("invitor_uin")]
    public LongId InvitorUin { get; set; }

    /// <summary>邀请者昵称</summary>
    [JsonPropertyName("invitor_nick")]
    public string InvitorNick { get; set; } = string.Empty;

    /// <summary>群号</summary>
    [JsonPropertyName("group_id")]
    public LongId GroupId { get; set; }

    /// <summary>群名称</summary>
    [JsonPropertyName("group_name")]
    public string GroupName { get; set; } = string.Empty;

    /// <summary>附言</summary>
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary>是否已处理</summary>
    [JsonPropertyName("checked")]
    public bool Checked { get; set; }

    /// <summary>操作者QQ</summary>
    [JsonPropertyName("actor")]
    public LongId Actor { get; set; }

    /// <summary>申请者昵称</summary>
    [JsonPropertyName("requester_nick")]
    public string RequesterNick { get; set; } = string.Empty;
}
