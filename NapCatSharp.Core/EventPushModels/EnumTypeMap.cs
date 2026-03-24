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
    public readonly static FrozenDictionary<MetaType, Type> MetaEventTypeMap;
    public readonly static FrozenDictionary<MessageType, Type> MessageEventTypeMap;
    public readonly static FrozenDictionary<RequestType, Type> RequestEventTypeMap;
    public readonly static FrozenDictionary<MessageSentType, Type> MessageSentTypeMap;
    /// <summary> <see cref="PostType"/> 到其子类型的映射 </summary>
    public readonly static FrozenDictionary<PostType, Type> PostTypeToSubTypeMap = new Dictionary<PostType, Type>()
    {
        { PostType.message, typeof(MessageType) },
        { PostType.meta_event, typeof(MetaType) },
        { PostType.request, typeof(RequestType) },
        { PostType.message_sent, typeof(MessageSentType) }
    }.ToFrozenDictionary();

    static EnumTypeMap()
    {
        var eventModelTypes = typeof(EnumTypeMap).Assembly.GetTypes().Where(
                t => t.IsAbstract == false && (t.BaseType?.IsGenericType ?? false) && 
                t.BaseType.GetGenericTypeDefinition() == typeof(EventBaseModelG<>)
        );

        Dictionary<MetaType, Type> metaEventTypeMapCache = [];
        Dictionary<MessageType, Type> messageEventTypeMapCache = [];
        Dictionary<RequestType, Type> requestEventTypeMapCache = [];
        Dictionary<MessageSentType, Type> messageSentTypeMapCache = [];

        var eventBaseModelType = eventModelTypes.Select(t => (th: t, baset: t.BaseType));

        foreach (var item in eventBaseModelType) {
            if (item.baset!.GetGenericArguments().Contains(typeof(MetaType))) {
                var th = (EventBaseModelG<MetaType>)Activator.CreateInstance(item.th)!;
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

            if (item.baset!.GetGenericArguments().Contains(typeof(MessageSentType))) {
                var th = (EventBaseModelG<MessageSentType>)Activator.CreateInstance(item.th)!;
                messageSentTypeMapCache[th.GetEnumValue()] = item.th;
            }
        }

        MetaEventTypeMap = metaEventTypeMapCache.ToFrozenDictionary();
        MessageEventTypeMap = messageEventTypeMapCache.ToFrozenDictionary();
        RequestEventTypeMap = requestEventTypeMapCache.ToFrozenDictionary();
        MessageSentTypeMap = messageSentTypeMapCache.ToFrozenDictionary();
    }

    public static FrozenDictionary<TEnum, Type>? GetMap<TEnum>()
        where TEnum : struct, Enum
    {
        if(typeof(TEnum) == typeof(MessageType)) {
            return (FrozenDictionary<TEnum, Type>)(object)MessageEventTypeMap;
        } else if(typeof(TEnum) == typeof(MetaType)) {
            return (FrozenDictionary<TEnum, Type>)(object)MetaEventTypeMap;
        } else if(typeof(TEnum) == typeof(RequestType)) {
            return (FrozenDictionary<TEnum, Type>)(object)RequestEventTypeMap;
        } else if(typeof(TEnum) == typeof(MessageSentType)) {
            return (FrozenDictionary<TEnum, Type>)(object)MessageSentTypeMap;
        }
        return null;
    }

    public static Type GetPostSubType(PostType postType)
    {
        if (PostTypeToSubTypeMap.TryGetValue(postType, out var type)) {
            return type;
        }
        return null!;
    }

#pragma warning disable CA2255 // 不应在库中使用 “ModuleInitializer” 属性
    [ModuleInitializer]
#pragma warning restore CA2255 // 不应在库中使用 “ModuleInitializer” 属性
    internal static void ModuleInitializer()
    {

    }

    extension(PostType postType)
    {
        public Type SubType => GetPostSubType(postType);
    }
}
