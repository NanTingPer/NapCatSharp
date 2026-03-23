using System.Text.Json.Serialization;
using NapCatSharp.RequestModels;

namespace NapCatSharp.RequestModels.SystemInterfaces;

/// <summary>处理可疑好友申请</summary>
public class SetDoubtFriendsAddRequest : RequestModelBase
{
    /// <summary>请求flag</summary>
    [JsonPropertyName("flag")]
    public required string Flag { get; set; }

    /// <summary>是否同意</summary>
    [JsonPropertyName("approve")]
    public required bool Approve { get; set; }

    public override string GetEndpoint()
    {
        return "/set_doubt_friends_add_request";
    }
}
