using System.Text.Json.Serialization;

namespace NapCatSharp.RequestModels.MessageInterfaces;

/// <summary>
/// 转发单条消息到私聊，注意不要设置<see cref="GroupId"/>
/// </summary>
public class ForwardFriendSingleMsg : RequestModelBase
{
    /// <summary>目标群号</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("group_id")]
    public StringId GroupId { get; set; }

    /// <summary>消息ID</summary>
    [JsonPropertyName("message_id")]
    public LongId MessageId { get; set; }

    /// <summary>目标用户QQ</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("user_id")]
    public StringId UserId { get; set; }

    public override string GetEndpoint()
    {
        return "/forward_friend_single_msg";
    }
}
