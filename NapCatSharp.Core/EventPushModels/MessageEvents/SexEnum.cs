using NapCatSharp.JsonConverter;
using System.Text.Json.Serialization;

namespace NapCatSharp.EventPushModels.MessageEvents;

[JsonConverter(typeof(SexEnumConverter))]
public enum SexEnum
{
    male,
    female,
    unknown
}
public class SexEnumConverter : EnumJsonConverter<SexEnum> { }