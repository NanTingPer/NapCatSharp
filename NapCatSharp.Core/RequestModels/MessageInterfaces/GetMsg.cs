using System.Text.Json.Serialization;

namespace NapCatSharp.RequestModels.MessageInterfaces;

/// <summary>
/// 根据消息 ID 获取消息详细信息
/// </summary>
public class GetMsg : RequestModelBase
{
    /// <summary>
    /// 消息ID
    /// </summary>
    [JsonPropertyName("message_id")]
    public required LongId MessageId { get; set; }
    public override string GetEndpoint()
    {
        return "/get_msg";
    }
}
