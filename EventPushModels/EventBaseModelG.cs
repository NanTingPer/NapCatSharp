namespace NapCatSharp.EventPushModels;

public abstract class EventBaseModelG<TEnum>  : EventBaseModel
    where TEnum : struct, Enum
{
    public abstract TEnum GetEnumValue();
}
