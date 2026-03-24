using System.Text.Json.Serialization;
using NapCatSharp.RequestModels;

namespace NapCatSharp.RequestModels.SystemInterfaces;

/// <summary>获取可疑好友申请列表</summary>
public class GetDoubtFriendsAddRequest : RequestModelBase
{
    /// <summary>获取数量</summary>
    [JsonPropertyName("count")]
    public required long Count { get; set; }

    public override string GetEndpoint()
    {
        return "/get_doubt_friends_add_request";
    }
}
