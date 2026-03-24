using NapCatSharp.OB11;
using System.Text.Json.Serialization;

namespace NapCatSharp.RequestModels.MessageInterfaces;

public class SendPrivateMsg : RequestModelBase
{
    /// <summary> 消息类型 (private/group) </summary>
    [JsonPropertyName("message_type")]
    public string MessageType { get; set; } = "private";

    [JsonPropertyName("user_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public required LongId UserId { get; set; }

    [JsonPropertyName("group_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public LongId GroupId { get; set; }

    /// <summary> OneBot 11 消息混合类型 </summary>
    [JsonPropertyName("message")]
    public required List<IOB11MessageModelFlag> Message { get; set; }

    /// <summary> 是否作为纯文本发送 </summary>
    [JsonPropertyName("auto_escape")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool AutoEscape { get; set; }

    /// <summary> 合并转发来源 </summary>
    [JsonPropertyName("source")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Source { get; set; }

    [JsonPropertyName("news")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? News {get;set; }

    /// <summary> 合并转发摘要 </summary>
    [JsonPropertyName("summary")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Summary { get; set; }

    /// <summary> 合并转发提示 </summary>
    [JsonPropertyName("prompt")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Prompt { get; set; }

    /// <summary>
    /// 自定义发送超时(毫秒)，覆盖自动计算值
    /// </summary>
    [JsonPropertyName("timeout")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? Timeout { get; set; }
    public override string GetEndpoint()
    {
        return "/send_private_msg";
    }
}
