using System.Text.Json.Serialization;
using NapCatSharp;

namespace NapCatSharp.RequestModels.GroupInterfaces;

/// <summary>获取群详细信息响应</summary>
public class GetGroupDetailInfoResponse : ResponseBaseModel<GetGroupDetailInfoResponse.DataInfo>
{
    public class DataInfo
    {
        /// <summary>群号</summary>
        [JsonPropertyName("group_id")]
        public LongId GroupId { get; set; }

        /// <summary>群名称</summary>
        [JsonPropertyName("group_name")]
        public string GroupName { get; set; } = string.Empty;

        /// <summary>成员数</summary>
        [JsonPropertyName("member_count")]
        public long MemberCount { get; set; }

        /// <summary>最大成员数</summary>
        [JsonPropertyName("max_member_count")]
        public long MaxMemberCount { get; set; }
    }
}
