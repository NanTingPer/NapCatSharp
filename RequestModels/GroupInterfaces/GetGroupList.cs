using System.Text.Json.Serialization;
using NapCatSharp.RequestModels;

namespace NapCatSharp.RequestModels.GroupInterfaces;

/// <summary>获取群列表</summary>
public class GetGroupList : RequestModelBase
{
    /// <summary>是否不使用缓存</summary>
    [JsonPropertyName("no_cache")]
    public bool? NoCache { get; set; }

    public override string GetEndpoint()
    {
        return "/get_group_list";
    }
}
