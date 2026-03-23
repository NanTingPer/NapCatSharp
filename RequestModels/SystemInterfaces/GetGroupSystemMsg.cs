using System.Text.Json.Serialization;
using NapCatSharp.RequestModels;

namespace NapCatSharp.RequestModels.SystemInterfaces;

/// <summary>获取群系统消息</summary>
public class GetGroupSystemMsg : RequestModelBase
{
    /// <summary>获取的消息数量</summary>
    [JsonPropertyName("count")]
    public required object Count { get; set; }

    public override string GetEndpoint()
    {
        return "/get_group_system_msg";
    }
}
