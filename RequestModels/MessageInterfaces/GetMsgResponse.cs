using NapCatSharp.OB11;
using System.Text.Json.Serialization;

namespace NapCatSharp.RequestModels.MessageInterfaces;

public class GetMsgResponse : RequestModelResponseBase<GetMsgResponse.DataInfo>
{
    public class DataInfo
    {
        [JsonPropertyName("time")]
        public LongId Time { get; set; }

        [JsonPropertyName("message_type")]
        public MessageType MessageType { get; set; }

        /// <summary> 消息Id </summary>
        [JsonPropertyName("message_id")]
        public LongId MessageId { get; set; }

        /// <summary> 真实Id </summary>
        [JsonPropertyName("real_id")]
        public LongId RealId { get; set; }

        /// <summary> 消息序号 </summary>
        [JsonPropertyName("message_seq")]
        public LongId MessageSeq { get; set; }

        [JsonPropertyName("sender")]
        public required SenderData Sender { get; set; }

        /// <summary> 消息内容 </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        /// <summary> 原始消息内容 </summary>
        [JsonPropertyName("raw_message")]
        public string RawMessage { get; set; } = string.Empty;

        [JsonPropertyName("font")]
        public LongId Font { get; set; }

        [JsonPropertyName("group_id")]
        public LongId GroupId { get; set; }

        [JsonPropertyName("user_id")]
        public LongId UserId { get; set; }

        /// <summary> 表情回应列表 </summary>
        [JsonPropertyName("emoji_likes_list")]
        public List<object> EmojiLikesList { get; set; } = [];
    }

    public class SenderData
    {
        [JsonPropertyName("user_id")]
        public LongId UserId { get; set; }

        [JsonPropertyName("nickname")]
        public string Nickname { get; set; } = string.Empty;
    }
}
