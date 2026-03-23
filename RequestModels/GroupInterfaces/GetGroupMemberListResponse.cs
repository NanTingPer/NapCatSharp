using System.Text.Json.Serialization;

namespace NapCatSharp.RequestModels.GroupInterfaces;

/// <summary>获取群成员列表响应</summary>
public class GetGroupMemberListResponse : ResponseBaseModel<List<GetGroupMemberListResponse.MemberInfo>>
{
    public class MemberInfo
    {
        /// <summary>群号</summary>
        [JsonPropertyName("group_id")]
        public LongId GroupId { get; set; }

        /// <summary>用户QQ号</summary>
        [JsonPropertyName("user_id")]
        public LongId UserId { get; set; }

        /// <summary>昵称</summary>
        [JsonPropertyName("nickname")]
        public string Nickname { get; set; } = string.Empty;

        /// <summary>群名片</summary>
        [JsonPropertyName("card")]
        public string Card { get; set; } = string.Empty;

        /// <summary>角色</summary>
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;
    }
}
