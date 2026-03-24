using System.Text.Json.Serialization;

namespace NapCatSharp.RequestModels.SystemInterfaces;

/// <summary>获取CSRF Token响应</summary>
public class GetCsrfTokenResponse : ResponseBaseModel<GetCsrfTokenResponse.DataInfo>
{
    public class DataInfo
    {
        /// <summary>Token值</summary>
        [JsonPropertyName("token")]
        public long Token { get; set; }
    }
}
