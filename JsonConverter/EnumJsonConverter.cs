using System.Text.Json.Serialization;

namespace NapCatSharp.JsonConverter;

public abstract class EnumJsonConverter<Tenum> : JsonConverter<Tenum>
    where Tenum : Enum
{

}
