using System.Text.Json.Serialization;
using NapCatSharp;
using NapCatSharp.RequestModels;

namespace NapCatSharp.RequestModels.GroupInterfaces;

/// <summary>设置群名称</summary>
public class SetGroupName : RequestModelBase
{
    /// <summary>群号</summary>
    [JsonPropertyName("group_id")]
    public required LongId GroupId { get; set; }

    /// <summary>群名称</summary>
    [JsonPropertyName("group_name")]
    public required string GroupName { get; set; }

    public override string GetEndpoint()
    {
        return "/set_group_name";
    }
}
