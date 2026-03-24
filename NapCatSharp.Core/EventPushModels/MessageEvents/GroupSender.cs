using System.Text.Json.Serialization;

namespace NapCatSharp.EventPushModels.MessageEvents;

/// <summary>
/// QQ 群成员信息
/// </summary>
public class GroupSender
{
    /// <summary>
    /// 发送者 QQ 号
    /// </summary>
    [JsonPropertyName("user_id")]
    public LongId UserId { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [JsonPropertyName("nickname")]
    public string Nickname { get; set; } = string.Empty;

    /// <summary>
    /// 群名片／备注
    /// </summary>
    [JsonPropertyName("card")]
    public string Card { get; set; } = string.Empty;

    /// <summary>
    /// 性别
    /// </summary>
    [JsonPropertyName("sex")]
    public SexEnum Sex { get; set; } = SexEnum.unknown;

    /// <summary>
    /// 年龄
    /// </summary>
    [JsonPropertyName("age")]
    public int Age { get; set; }

    /// <summary>
    /// 地区
    /// </summary>
    [JsonPropertyName("area")]
    public string Area { get; set; } = string.Empty;

    /// <summary>
    /// 成员等级
    /// </summary>
    [JsonPropertyName("level")]
    public string Level { get; set; } = string.Empty;

    /// <summary>
    /// 角色
    /// </summary>
    [JsonPropertyName("role")]
    public RoleEnum Role { get; set; }

    /// <summary>
    /// 专属头衔
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;
}
