using System.Text.Json.Serialization;

namespace NapCatSharp.RequestModels.MessageInterfaces;

/// <summary>
/// 撤回消息
/// </summary>
public class DeleteMsg : RequestModelBase
{
    [JsonPropertyName("message_id")]
    public required LongId MessageId { get; set; }
    public override string GetEndpoint()
    {
        return "/delete_msg";
    }
}
