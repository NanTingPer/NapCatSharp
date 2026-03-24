using NapCatSharp.OB11;
using System.Text.Json.Serialization;

namespace NapCatSharp.OB11.OB11MessageModels;

/// <summary> Markdown消息段 </summary>
public class Markdown : OB11MessageModelBase<Markdown.OB11MessageMarkdown, Markdown>
{
    public class OB11MessageMarkdown
    {
        /// <summary> Markdown内容 </summary>
        [JsonPropertyName("content")]
        public required string Content { get; set; }
    }
}
