using NapCatSharp.OB11;
using System.Text.Json.Serialization;

namespace NapCatSharp.OB11.OB11MessageModels;

/// <summary> 合并转发消息节点 </summary>
public class Node : OB11MessageModelBase<Node.OB11MessageNode, Node>
{
    public class OB11MessageNode
    {
        /// <summary> 转发消息ID </summary>
        [JsonPropertyName("id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public StringId? Id { get; set; }

        /// <summary> 发送者QQ号 </summary>
        [JsonPropertyName("user_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public StringId? UserId { get; set; }

        /// <summary> 发送者QQ号(兼容go-cqhttp) </summary>
        [JsonPropertyName("uin")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public StringId? Uin { get; set; }

        /// <summary> 发送者昵称 </summary>
        [JsonPropertyName("nickname")]
        public required string Nickname { get; set; }

        /// <summary> 发送者昵称(兼容go-cqhttp) </summary>
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Name { get; set; }

        /// <summary> 消息内容 (OB11MessageMixType) </summary>
        [JsonPropertyName("content")]
        public required object Content { get; set; }

        /// <summary> 消息来源 </summary>
        [JsonPropertyName("source")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Source { get; set; }

        /// <summary> 新闻列表 </summary>
        [JsonPropertyName("news")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<NewsItem>? News { get; set; }

        /// <summary> 摘要 </summary>
        [JsonPropertyName("summary")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Summary { get; set; }

        /// <summary> 提示 </summary>
        [JsonPropertyName("prompt")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Prompt { get; set; }

        /// <summary> 时间 </summary>
        [JsonPropertyName("time")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Time { get; set; }

        /// <summary> 新闻项 </summary>
        public class NewsItem
        {
            /// <summary> 新闻文本 </summary>
            [JsonPropertyName("text")]
            public required string Text { get; set; }
        }
    }
}