using System.Text.Json.Serialization;
using NapCatSharp;

namespace NapCatSharp.RequestModels.GroupInterfaces;

/// <summary>获取群精华消息响应</summary>
public class GetEssenceMsgListResponse : ResponseBaseModel<List<GetEssenceMsgListResponse.EssenceMsgInfo>>
{
    public class EssenceMsgInfo
    {
        /// <summary>消息ID</summary>
        [JsonPropertyName("message_id")]
        public long MessageId { get; set; }

        /// <summary>发送者ID</summary>
        [JsonPropertyName("sender_id")]
        public LongId SenderId { get; set; }

        /// <summary>发送者昵称</summary>
        [JsonPropertyName("sender_nick")]
        public string SenderNick { get; set; } = string.Empty;

        /// <summary>操作者ID</summary>
        [JsonPropertyName("operator_id")]
        public LongId OperatorId { get; set; }

        /// <summary>操作者昵称</summary>
        [JsonPropertyName("operator_nick")]
        public string OperatorNick { get; set; } = string.Empty;

        /// <summary>操作时间</summary>
        [JsonPropertyName("operator_time")]
        public long OperatorTime { get; set; }

        /// <summary>精华内容</summary>
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;
    }
}
