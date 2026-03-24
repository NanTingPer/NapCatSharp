using NapCatSharp.EventPushModels;
using NapCatSharp.EventPushModels.MessageEvents;
using NapCatSharp.EventPushModels.MessageSentEvents;
using NapCatSharp.EventPushModels.MetaEvents;
using NapCatSharp.EventPushModels.RequestEvents;

namespace NapCatSharp.Mod;

public static class ModManager
{
    public static void MetaLifecycle(EventBaseModel eventBaseModel)
    {
        if (eventBaseModel is not Lifecycle lifecycle) return;
    }
    public static void MetaHeartbeat(EventBaseModel eventBaseModel)
    {
        if (eventBaseModel is not Heartbeat heartbeat) return;
    }
    public static void MessagePrivate(EventBaseModel eventBaseModel)
    {
        if (eventBaseModel is not PrivateMessage message) return;
    }
    public static void MessageGroup(EventBaseModel eventBaseModel)
    {
        if (eventBaseModel is not GroupMessage message) return;
    }
    public static void RequestFriend(EventBaseModel eventBaseModel)
    {
        if (eventBaseModel is not AddFriend request) return;
    }
    public static void RequestGroup(EventBaseModel eventBaseModel)
    {
        if (eventBaseModel is not AddGroup request) return;
    }
    public static void MessageSentSelf(EventBaseModel eventBaseModel)
    {
        if (eventBaseModel is not Self self) return;
    }
}