using NapCatSharp.JsonConverter;
using System.Text.Json.Serialization;

namespace NapCatSharp.EventPushModels.MessageEvents;

/// <summary>
/// 群成员角色枚举
/// </summary>
[JsonConverter(typeof(RoleConverter))]
public enum RoleEnum
{
    owner,
    admin,
    member
}

public class RoleConverter : EnumJsonConverter<RoleEnum>{}