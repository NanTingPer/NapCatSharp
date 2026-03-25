using NapCatSharp.EventPushModels;
using NapCatSharp.EventPushModels.MessageEvents;
using NapCatSharp.EventPushModels.MessageSentEvents;
using NapCatSharp.EventPushModels.MetaEvents;
using NapCatSharp.EventPushModels.RequestEvents;

namespace NapCatSharp.Mod;

public class ModManager
{
    public void MetaLifecycle(EventBaseModel eventBaseModel)
    {
        if (eventBaseModel is not Lifecycle lifecycle) return;
    }
    public void MetaHeartbeat(EventBaseModel eventBaseModel)
    {
        if (eventBaseModel is not Heartbeat heartbeat) return;
    }
    public void MessagePrivate(EventBaseModel eventBaseModel)
    {
        if (eventBaseModel is not PrivateMessage message) return;
    }
    public void MessageGroup(EventBaseModel eventBaseModel)
    {
        if (eventBaseModel is not GroupMessage message) return;
    }
    public void RequestFriend(EventBaseModel eventBaseModel)
    {
        if (eventBaseModel is not AddFriend request) return;
    }
    public void RequestGroup(EventBaseModel eventBaseModel)
    {
        if (eventBaseModel is not AddGroup request) return;
    }
    public void MessageSentSelf(EventBaseModel eventBaseModel)
    {
        if (eventBaseModel is not Self self) return;
    }
}

public interface IModManager
{
    public void MetaLifecycle(EventBaseModel eventBaseModel);
    public void MetaHeartbeat(EventBaseModel eventBaseModel);
    public void MessagePrivate(EventBaseModel eventBaseModel);
    public void MessageGroup(EventBaseModel eventBaseModel);
    public void RequestFriend(EventBaseModel eventBaseModel);
    public void RequestGroup(EventBaseModel eventBaseModel);
    public void MessageSentSelf(EventBaseModel eventBaseModel);
}