using System.Text.Json.Serialization;

namespace NapCatSharp.EventPushModels.MessageEvents;

/// <summary>
/// 需要注意的是，sender 中的各字段是尽最大努力提供的，也就是说，不保证每个字段都一定存在，也不保证存在的字段都是完全正确的（缓存可能过期）。
/// </summary>
public class PrivateSender
{
    /// <summary>
    /// QQ号
    /// </summary>
    [JsonPropertyName("user_id")]
    public LongId UserId { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [JsonPropertyName("nickname")]
    public string Nickname { get; set; } = string.Empty;

    [JsonPropertyName("Sex")]
    public SexEnum Sex { get; set; } = SexEnum.unknown;
}
