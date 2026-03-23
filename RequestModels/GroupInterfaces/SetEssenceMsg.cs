using System.Text.Json.Serialization;
using NapCatSharp.RequestModels;

namespace NapCatSharp.RequestModels.GroupInterfaces;

/// <summary>设置精华消息</summary>
public class SetEssenceMsg : RequestModelBase
{
    /// <summary>消息ID</summary>
    [JsonPropertyName("message_id")]
    public required LongId MessageId { get; set; }

    public override string GetEndpoint()
    {
        return "/set_essence_msg";
    }
}
