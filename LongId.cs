using NapCatSharp.JsonConverter;
using System.Text.Json.Serialization;

namespace NapCatSharp;

/// <summary>
/// long型iD
/// <br/> 拥有string -> long -> string的隐士类型转换
/// </summary>
[JsonConverter(typeof(LongIdConverter))]
public struct LongId
{
    public string String;
    public long Long;

    public static implicit operator LongId(string string_) => new LongId { String = string_, Long = long.Parse(string_) };
    public static implicit operator LongId(long long_) => new LongId { String = long_.ToString(), Long = long_ };
    public static implicit operator string(LongId sid) => sid.String;
    public static implicit operator long(LongId sid) => sid.Long;
    public static implicit operator StringId(LongId sid) => new StringId { String = sid.String, Long = sid.Long };
}
