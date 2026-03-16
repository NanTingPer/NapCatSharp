using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace NapCatSharp.EventPushModels;

public static class EnumTypeMap
{
    public readonly static FrozenDictionary<MetaEventType, Type> MetaEventTypeMap;

    static EnumTypeMap()
    {
        var eventModelTypes = typeof(EnumTypeMap).Assembly.GetTypes().Where(
                t => t.IsAbstract == false && (t.BaseType?.IsGenericType ?? false) && 
                t.BaseType.GetGenericTypeDefinition() == typeof(EventBaseModelG<>)
        );

        Dictionary<MetaEventType, Type> metaEventTypeMapCache = [];

        var eventBaseModelType = eventModelTypes.Select(t => (th: t, baset: t.BaseType));
        foreach (var item in eventBaseModelType) {
            if (item.baset!.GetGenericArguments().Contains(typeof(MetaEventType))) {
                var th = (EventBaseModelG<MetaEventType>)Activator.CreateInstance(item.th)!;
                metaEventTypeMapCache[th.GetEnumValue()] = item.th;
            }
        }

        MetaEventTypeMap = metaEventTypeMapCache.ToFrozenDictionary();
    }

#pragma warning disable CA2255 // 不应在库中使用 “ModuleInitializer” 属性
    [ModuleInitializer]
#pragma warning restore CA2255 // 不应在库中使用 “ModuleInitializer” 属性
    internal static void ModuleInitializer()
    {

    }
}
