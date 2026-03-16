using NapCatSharp.JsonConverter;
using System.Text.Json.Serialization;

namespace NapCatSharp.OB11;

/// <summary>
/// OB11消息类似抽象
/// </summary>
/// <typeparam name="TModel"> Data </typeparam>
/// <typeparam name="This"></typeparam>
public abstract class OB11MessageModelBase<TModel, This>: IOB11MessageModelFlag
    where TModel : class
    where This : OB11MessageModelBase<TModel, This>
{
    public OB11MessageModelBase()
    {
        Type = GetMessageType();
    }

    /// <summary>
    /// 消息类型
    /// </summary>
    [JsonPropertyName("type")]
    public OB11MessageType Type { get{ if(field == default) return GetMessageType(); else return field; } set => field = value; }

    /// <summary>
    /// 消息数据
    /// </summary>
    [JsonPropertyName("data")]
    public required TModel Data { get; set; }

    public virtual OB11MessageType GetMessageType()
    {
        var typename = GetType().Name;
        var keyName = typename.ToLower();
        if (OB11MessageType.typeNameMap.TryGetValue(keyName, out var reslut)) {
            return reslut;
        }
        return default;
    }

    public static This Create(TModel data)
    {
        This @this = Activator.CreateInstance<This>();
        @this.Data = data;
        return @this;
    }
}

[JsonConverter(typeof(OB11MessageModelFlagConver))]
public interface IOB11MessageModelFlag
{
    OB11MessageType GetMessageType();
}