using System.Text.Json.Serialization;
using NapCatSharp;

namespace NapCatSharp.RequestModels.GroupInterfaces;

/// <summary>获取群公告响应</summary>
public class GetGroupNoticeResponse : ResponseBaseModel<List<GetGroupNoticeResponse.NoticeInfo>>
{
    public class NoticeInfo
    {
        /// <summary>公告ID</summary>
        [JsonPropertyName("notice_id")]
        public string NoticeId { get; set; } = string.Empty;

        /// <summary>发送者ID</summary>
        [JsonPropertyName("sender_id")]
        public LongId SenderId { get; set; }

        /// <summary>发布时间</summary>
        [JsonPropertyName("publish_time")]
        public long PublishTime { get; set; }

        /// <summary>消息内容</summary>
        [JsonPropertyName("message")]
        public MessageInfo Message { get; set; } = new();

        public class MessageInfo
        {
            /// <summary>文本内容</summary>
            [JsonPropertyName("text")]
            public string Text { get; set; } = string.Empty;

            /// <summary>图片列表</summary>
            [JsonPropertyName("image")]
            public List<string> Image { get; set; } = [];
        }
    }
}
