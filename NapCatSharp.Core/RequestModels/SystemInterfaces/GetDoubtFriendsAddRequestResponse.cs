using System.Text.Json.Serialization;

namespace NapCatSharp.RequestModels.SystemInterfaces;

/// <summary>获取可疑好友申请响应</summary>
public class GetDoubtFriendsAddRequestResponse : ResponseBaseModel<List<GetDoubtFriendsAddRequestResponse.DataInfo>>
{
    public class DataInfo
    {
        /// <summary>用户ID</summary>
        [JsonPropertyName("user_id")]
        public long UserId { get; set; }

        /// <summary>昵称</summary>
        [JsonPropertyName("nickname")]
        public string Nickname { get; set; } = string.Empty;

        /// <summary>年龄</summary>
        [JsonPropertyName("age")]
        public long Age { get; set; }

        /// <summary>性别</summary>
        [JsonPropertyName("sex")]
        public string Sex { get; set; } = string.Empty;

        /// <summary>申请理由</summary>
        [JsonPropertyName("reason")]
        public string Reason { get; set; } = string.Empty;

        /// <summary>请求flag</summary>
        [JsonPropertyName("flag")]
        public string Flag { get; set; } = string.Empty;
    }
}
