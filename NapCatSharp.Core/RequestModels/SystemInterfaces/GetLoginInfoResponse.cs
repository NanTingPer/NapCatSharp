using System.Text.Json.Serialization;

namespace NapCatSharp.RequestModels.SystemInterfaces;

/// <summary>获取登录信息响应</summary>
public class GetLoginInfoResponse : ResponseBaseModel<GetLoginInfoResponse.DataInfo>
{
    public class DataInfo
    {
        /// <summary>用户ID</summary>
        [JsonPropertyName("user_id")]
        public long UserId { get; set; }

        /// <summary>昵称</summary>
        [JsonPropertyName("nickname")]
        public string Nickname { get; set; } = string.Empty;
    }
}
