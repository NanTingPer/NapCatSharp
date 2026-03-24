using NapCatSharp.OB11;
using System.Text.Json.Serialization;

namespace NapCatSharp.RequestModels.SystemInterfaces;

/// <summary>获取群系统消息响应</summary>
public class GetGroupSystemMsgResponse : ResponseBaseModel<GetGroupSystemMsgResponse.DataInfo>
{
    public class DataInfo
    {
        /// <summary>邀请请求列表</summary>
        [JsonPropertyName("invited_requests")]
        public List<OB11Notify> InvitedRequests { get; set; } = [];

        /// <summary>邀请请求</summary>
        [JsonPropertyName("InvitedRequest")]
        public List<OB11Notify> InvitedRequest { get; set; } = [];

        /// <summary>加入请求列表</summary>
        [JsonPropertyName("join_requests")]
        public List<OB11Notify> JoinRequests { get; set; } = [];
    }
}
