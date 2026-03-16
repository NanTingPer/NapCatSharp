using System.Text.Json.Serialization;

namespace NapCatSharp.RequestModels.OB11MessageModels;

/// <summary> 图片消息段 </summary>
public class Image : OB11MessageModelBase<Image.OB11MessageImage, Image>
{
    public class OB11MessageImage
    {
        /// <summary>
        /// 文件路径/URL/file:///
        /// <br/>file:///base64
        /// </summary>
        [JsonPropertyName("file")]
        public required string File { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        [JsonPropertyName("path")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Path { get; set; }
        /// <summary>
        /// 文件URL
        /// </summary>
        [JsonPropertyName("url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Url { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Name { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>
        [JsonPropertyName("thumb")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Thumb { get; set; }
        /// <summary>
        /// 图片摘要
        /// </summary>
        [JsonPropertyName("summary")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Summary { get; set; }
        /// <summary>
        /// 图片子类型
        /// </summary>
        [JsonPropertyName("sub_type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public LongId SubType { get; set; }
    }
}
