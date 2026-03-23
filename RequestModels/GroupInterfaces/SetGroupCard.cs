using System.Text.Json.Serialization;
using NapCatSharp;
using NapCatSharp.RequestModels;

namespace NapCatSharp.RequestModels.GroupInterfaces;

/// <summary>设置群名片</summary>
public class SetGroupCard : RequestModelBase
{
    /// <summary>群号</summary>
    [JsonPropertyName("group_id")]
    public required LongId GroupId { get; set; }

    /// <summary>用户QQ</summary>
    [JsonPropertyName("user_id")]
    public required LongId UserId { get; set; }

    /// <summary>群名片</summary>
    [JsonPropertyName("card")]
    public string? Card { get; set; }

    public override string GetEndpoint()
    {
        return "/set_group_card";
    }
}
