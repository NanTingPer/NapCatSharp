using System.Text.Json.Serialization;

namespace NapCatSharp.RequestModels.SystemInterfaces;

/// <summary>获取登录凭证响应</summary>
public class GetCredentialsResponse : ResponseBaseModel<GetCredentialsResponse.DataInfo>
{
    public class DataInfo
    {
        /// <summary>Cookies</summary>
        [JsonPropertyName("cookies")]
        public string Cookies { get; set; } = string.Empty;

        /// <summary>Token值</summary>
        [JsonPropertyName("token")]
        public long Token { get; set; }
    }
}
