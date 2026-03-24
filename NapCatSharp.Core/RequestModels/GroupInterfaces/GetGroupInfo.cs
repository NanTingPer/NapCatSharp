using System.Text.Json.Serialization;
using NapCatSharp;
using NapCatSharp.RequestModels;

namespace NapCatSharp.RequestModels.GroupInterfaces;

/// <summary>获取群信息</summary>
public class GetGroupInfo : RequestModelBase
{
    /// <summary>群号</summary>
    [JsonPropertyName("group_id")]
    public required LongId GroupId { get; set; }

    public override string GetEndpoint()
    {
        return "/get_group_info";
    }
}
