using System.Text.Json.Serialization;
using NapCatSharp;
using NapCatSharp.RequestModels;

namespace NapCatSharp.RequestModels.GroupInterfaces;

/// <summary>获取群成员列表</summary>
public class GetGroupMemberList : RequestModelBase
{
    /// <summary>群号</summary>
    [JsonPropertyName("group_id")]
    public required LongId GroupId { get; set; }

    /// <summary>是否不使用缓存</summary>
    [JsonPropertyName("no_cache")]
    public bool? NoCache { get; set; }

    public override string GetEndpoint()
    {
        return "/get_group_member_list";
    }
}
