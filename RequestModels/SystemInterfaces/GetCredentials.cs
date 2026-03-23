using System.Text.Json.Serialization;
using NapCatSharp.RequestModels;

namespace NapCatSharp.RequestModels.SystemInterfaces;

/// <summary>获取登录凭证</summary>
public class GetCredentials : RequestModelBase
{
    /// <summary>需要获取cookies的域名</summary>
    [JsonPropertyName("domain")]
    public required string Domain { get; set; }

    public override string GetEndpoint()
    {
        return "/get_credentials";
    }
}
