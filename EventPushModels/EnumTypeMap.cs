using NapCatSharp.EventPushModels.Enums;
using NapCatSharp.OB11;
using System.Collections.Frozen;
using System.Runtime.CompilerServices;

namespace NapCatSharp.EventPushModels;

/// <summary>
/// 枚举类型到具体类型的映射类型
/// <br/> 如元事件有两种类型，那么每个枚举就代表一个具体的实体类型
/// </summary>
public static class EnumTypeMap
{
    public readonly static FrozenDictionary<MetaEventType, Type> MetaEventTypeMap;
    public readonly static FrozenDictionary<MessageType, Type> MessageEventTypeMap;
    public readonly static FrozenDictionary<RequestType, Type> RequestEventTypeMap;

    static EnumTypeMap()
    {
        var eventModelTypes = typeof(EnumTypeMap).Assembly.GetTypes().Where(
                t => t.IsAbstract == false && (t.BaseType?.IsGenericType ?? false) && 
                t.BaseType.GetGenericTypeDefinition() == typeof(EventBaseModelG<>)
        );

        Dictionary<MetaEventType, Type> metaEventTypeMapCache = [];
        Dictionary<MessageType, Type> messageEventTypeMapCache = [];
        Dictionary<RequestType, Type> requestEventTypeMapCache = [];

        var eventBaseModelType = eventModelTypes.Select(t => (th: t, baset: t.BaseType));

        foreach (var item in eventBaseModelType) {
            if (item.baset!.GetGenericArguments().Contains(typeof(MetaEventType))) {
                var th = (EventBaseModelG<MetaEventType>)Activator.CreateInstance(item.th)!;
                metaEventTypeMapCache[th.GetEnumValue()] = item.th;
            }

            if (item.baset!.GetGenericArguments().Contains(typeof(MessageType))) {
                var th = (EventBaseModelG<MessageType>)Activator.CreateInstance(item.th)!;
                messageEventTypeMapCache[th.GetEnumValue()] = item.th;
            }

            if (item.baset!.GetGenericArguments().Contains(typeof(RequestType))) {
                var th = (EventBaseModelG<RequestType>)Activator.CreateInstance(item.th)!;
                requestEventTypeMapCache[th.GetEnumValue()] = item.th;
            }
        }

        MetaEventTypeMap = metaEventTypeMapCache.ToFrozenDictionary();
        MessageEventTypeMap = messageEventTypeMapCache.ToFrozenDictionary();
        RequestEventTypeMap = requestEventTypeMapCache.ToFrozenDictionary();
    }

    public static FrozenDictionary<TEnum, Type>? GetMap<TEnum>()
        where TEnum : struct, Enum
    {
        if(typeof(TEnum) == typeof(MessageType)) {
            return (FrozenDictionary<TEnum, Type>)(object)MessageEventTypeMap;
        } else if(typeof(TEnum) == typeof(MetaEventType)) {
            return (FrozenDictionary<TEnum, Type>)(object)MetaEventTypeMap;
        } else if(typeof(TEnum) == typeof(RequestType)) {
            return (FrozenDictionary<TEnum, Type>)(object)RequestEventTypeMap;
        }
        return null;
    }

#pragma warning disable CA2255 // 不应在库中使用 “ModuleInitializer” 属性
    [ModuleInitializer]
#pragma warning restore CA2255 // 不应在库中使用 “ModuleInitializer” 属性
    internal static void ModuleInitializer()
    {

    }
}
