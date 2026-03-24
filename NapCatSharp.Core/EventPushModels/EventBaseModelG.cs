namespace NapCatSharp.EventPushModels;

/// <summary>
/// 会<see cref="NapCatSharp.Core.NapCatHttpSocket"/>中判断事件类型，从而下发对应的事件。
/// <br/> 在确定具体大的事件类型后，才会获取具体的枚举类型，从而获取实体类型。
/// <br/> 最终实体类型从<see cref="EnumTypeMap"/>中获取，里面包含了相关映射。
/// <br/> 说到底，TEnum就是除了Post外，能确定此类型的枚举类型，或者说是SubType
/// </summary>
/// <typeparam name="TEnum"></typeparam>
public abstract class EventBaseModelG<TEnum>  : EventBaseModel
    where TEnum : struct, Enum
{
    public abstract TEnum GetEnumValue();
}
