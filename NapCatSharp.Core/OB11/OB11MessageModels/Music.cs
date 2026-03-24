using NapCatSharp.OB11;
using System.Text.Json.Serialization;

namespace NapCatSharp.OB11.OB11MessageModels;

/// <summary> 音乐消息段 </summary>
public class Music : OB11MessageModelBase<Music.OB11MessageMusic, Music>
{
    public class OB11MessageMusic
    {
        /// <summary> 音乐类型 </summary>
        [JsonPropertyName("type")]
        public required string Type { get; set; }

        /// <summary> 音乐ID </summary>
        [JsonPropertyName("id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public LongId? Id { get; set; }

        /// <summary> 音频URL (自定义音乐) </summary>
        [JsonPropertyName("url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Url { get; set; }

        /// <summary> 音频地址 (自定义音乐) </summary>
        [JsonPropertyName("audio")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Audio { get; set; }

        /// <summary> 标题 (自定义音乐) </summary>
        [JsonPropertyName("title")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Title { get; set; }

        /// <summary> 图片URL (自定义音乐) </summary>
        [JsonPropertyName("image")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Image { get; set; }

        /// <summary> 内容 (自定义音乐) </summary>
        [JsonPropertyName("content")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Content { get; set; }
    }
}
