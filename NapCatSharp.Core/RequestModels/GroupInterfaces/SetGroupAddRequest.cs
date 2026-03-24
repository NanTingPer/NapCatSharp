using System.Text.Json.Serialization;
using NapCatSharp.RequestModels;

namespace NapCatSharp.RequestModels.GroupInterfaces;

/// <summary>处理加群请求</summary>
public class SetGroupAddRequest : RequestModelBase
{
    /// <summary>请求flag</summary>
    [JsonPropertyName("flag")]
    public required string Flag { get; set; }

    /// <summary>是否同意</summary>
    [JsonPropertyName("approve")]
    public required bool Approve { get; set; }

    /// <summary>拒绝理由</summary>
    [JsonPropertyName("reason")]
    public string? Reason { get; set; }

    /// <summary>搜索通知数量</summary>
    [JsonPropertyName("count")]
    public long? Count { get; set; }

    public override string GetEndpoint()
    {
        return "/set_group_add_request";
    }
}
