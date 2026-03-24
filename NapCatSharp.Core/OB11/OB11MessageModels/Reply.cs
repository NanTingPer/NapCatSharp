using NapCatSharp.OB11;
using System.Text.Json.Serialization;

namespace NapCatSharp.OB11.OB11MessageModels;
/// <summary> 回复消息段 </summary>
public class Reply : OB11MessageModelBase<Reply.OB11MessageReply, Reply>
{
    public class OB11MessageReply
    {
        /// <summary> 消息ID的短ID映射 </summary>
        [JsonPropertyName("id")]
        public required StringId Id { get; set; }  // 改为 StringId

        /// <summary> 消息序列号，优先使用 </summary>
        [JsonPropertyName("seq")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public LongId Seq { get; set; }
    }
}