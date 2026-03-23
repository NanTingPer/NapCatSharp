using System.Text.Json.Serialization;
using NapCatSharp;
using NapCatSharp.RequestModels;

namespace NapCatSharp.RequestModels.GroupInterfaces;

/// <summary>退出群组</summary>
public class SetGroupLeave : RequestModelBase
{
    /// <summary>群号</summary>
    [JsonPropertyName("group_id")]
    public required LongId GroupId { get; set; }

    /// <summary>是否解散 暂无作用</summary>
    [JsonPropertyName("is_dismiss")]
    public bool? IsDismiss { get; set; }

    public override string GetEndpoint()
    {
        return "/set_group_leave";
    }
}
