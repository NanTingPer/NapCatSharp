using NapCatSharp.JsonConverter;
using System.Text.Json.Serialization;

namespace NapCatSharp.RequestModels;

/// <summary> 
/// string型id 
/// <br/> 拥有string -> long -> string的隐士类型转换
/// </summary>
[JsonConverter(typeof(StringIdConverter))]
public struct StringId
{
    public string String;
    public long Long;

    public static implicit operator StringId(string string_) => new StringId { String = string_, Long = long.Parse(string_) };
    public static implicit operator StringId(long long_) => new StringId { String = long_.ToString(), Long = long_ };
    public static implicit operator string(StringId sid) => sid.String;
    public static implicit operator long(StringId sid) => sid.Long;
    public static implicit operator LongId(StringId sid) => new LongId{ String = sid.String, Long = sid.Long };
}
