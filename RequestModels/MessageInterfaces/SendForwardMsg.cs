using NapCatSharp.OB11;
using NapCatSharp.OB11.OB11MessageModels;
using System.Text.Json.Serialization;

namespace NapCatSharp.RequestModels.MessageInterfaces;

/// <summary>
/// 发送合并转发消息
/// </summary>
public class SendForwardMsg : RequestModelBase
{
    [JsonPropertyName("message_type")]
    public required MessageType MessageType { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("user_id")]
    public StringId? UserId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("group_id")]
    public StringId? GroupId { get; set; }

    [JsonPropertyName("message")]
    public List<Node> Message { get; set; } = [];

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("auto_escape")]
    public bool AutoEscape { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("source")]
    public string? Source { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("news")]
    public List<string>? News { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("summary")]
    public string? Summary { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("prompt")]
    public string? Prompt { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("timeout")]
    public long Timeout { get; set; }

    public override string GetEndpoint()
    {
        return "/send_forward_msg";
    }
}
