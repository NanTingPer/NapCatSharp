using System.Text.Json;
using System.Text.Json.Serialization;
namespace NapCatSharp.RequestModels.MessageInterfaces;

/// <summary>
/// 标记群聊已读
/// </summary>
public class MarkGroupMsgAsRead : RequestModelBase
{
    /// <summary>
    /// 群号
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("group_id")]
    public LongId GroupId { get; set; }

    /// <summary>
    /// 消息ID
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("message_id")]
    public LongId MessageId { get; set; }

    /// <summary>
    /// 用户QQ
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("user_id")]
    public LongId UserId { get; set; }

    public override string GetEndpoint()
    {
        return "/mark_group_msg_as_read";
    }
}
