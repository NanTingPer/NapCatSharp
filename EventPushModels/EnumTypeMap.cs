using NapCatSharp.EventPushModels.MessageEvents;
using System.Collections.Frozen;
using System.Runtime.CompilerServices;

namespace NapCatSharp.EventPushModels;

public static class EnumTypeMap
{
    public readonly static FrozenDictionary<MetaEventType, Type> MetaEventTypeMap;
    public readonly static FrozenDictionary<MessageType, Type> MessageEventTypeMap;

    static EnumTypeMap()
    {
        var eventModelTypes = typeof(EnumTypeMap).Assembly.GetTypes().Where(
                t => t.IsAbstract == false && (t.BaseType?.IsGenericType ?? false) && 
                t.BaseType.GetGenericTypeDefinition() == typeof(EventBaseModelG<>)
        );

        Dictionary<MetaEventType, Type> metaEventTypeMapCache = [];
        Dictionary<MessageType, Type> messageEventTypeMapCache = [];

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
        }

        MetaEventTypeMap = metaEventTypeMapCache.ToFrozenDictionary();
        MessageEventTypeMap = messageEventTypeMapCache.ToFrozenDictionary();
    }

    public static FrozenDictionary<TEnum, Type>? GetMap<TEnum>()
        where TEnum : struct, Enum
    {
        if(typeof(TEnum) == typeof(MessageType)) {
            return (FrozenDictionary<TEnum, Type>)(object)MessageEventTypeMap;
        } else if(typeof(TEnum) == typeof(MetaEventType)) {
            return (FrozenDictionary<TEnum, Type>)(object)MetaEventTypeMap;
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
