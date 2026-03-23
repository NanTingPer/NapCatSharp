using NapCatSharp.OB11;
using System.Text.Json.Serialization;

namespace NapCatSharp.OB11.OB11MessageModels;

/// <summary> 视频消息段 </summary>
public class Video : OB11MessageModelBase<Video.OB11MessageVideo, Video>
{
    public class OB11MessageVideo
    {
        /// <summary> 文件路径/URL/file:///<br/>file:///base64 </summary>
        [JsonPropertyName("file")]
        public required string File { get; set; }

        /// <summary> 文件路径 </summary>
        [JsonPropertyName("path")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Path { get; set; }

        /// <summary> 文件URL </summary>
        [JsonPropertyName("url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Url { get; set; }

        /// <summary> 文件名 </summary>
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Name { get; set; }

        /// <summary> 缩略图 </summary>
        [JsonPropertyName("thumb")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Thumb { get; set; }
    }
}
