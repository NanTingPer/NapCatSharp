using NapCatSharp.OB11;
using System.Text.Json.Serialization;

namespace NapCatSharp.OB11.OB11MessageModels;

/// <summary> 合并转发消息段 </summary>
public class Forward : OB11MessageModelBase<Forward.OB11MessageForward, Forward>
{
    public class OB11MessageForward
    {
        /// <summary> 合并转发ID </summary>
        [JsonPropertyName("id")]
        public required StringId Id { get; set; }

        /// <summary> 消息内容 (OB11Message[]) </summary>
        [JsonPropertyName("content")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<IOB11MessageModelFlag>? Content { get; set; }
    }
}