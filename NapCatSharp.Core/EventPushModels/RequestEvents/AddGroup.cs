using NapCatSharp.EventPushModels.Enums;
using NapCatSharp.JsonConverter;
using System.Text.Json.Serialization;

namespace NapCatSharp.EventPushModels.RequestEvents;

public class AddGroup : EventBaseModelG<RequestType>
{
    [JsonPropertyName("request_type")]
    public RequestType RequestType { get; set; } = RequestType.friend;
    [JsonPropertyName("user_id")]
    public LongId UserId { get; set; }

    [JsonPropertyName("sub_type")]
    public AddGroupSubType SubType { get; set; }

    [JsonPropertyName("group_id")]
    public LongId GroupId { get; set; }

    /// <summary>
    /// 验证消息
    /// </summary>
    [JsonPropertyName("comment")]
    public string Comment { get; set; } = string.Empty;
    
    /// <summary>
    /// 请求flage，处理请求的API需要使用
    /// </summary>
    [JsonPropertyName("flag")]
    public string? Flag { get; set; }
    public override RequestType GetEnumValue()
    {
        return RequestType.group;
    }

    [JsonConverter(typeof(AddGroupSubTypeConverter))]
    public enum AddGroupSubType
    {
        add,
        invite
    }

    public class AddGroupSubTypeConverter : EnumJsonConverter<AddGroupSubType>{}
}
