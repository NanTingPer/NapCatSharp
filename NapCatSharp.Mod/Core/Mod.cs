using NapCatSharp.EventPushModels.MessageEvents;
using NapCatSharp.EventPushModels.MessageSentEvents;
using NapCatSharp.EventPushModels.MetaEvents;
using NapCatSharp.EventPushModels.RequestEvents;

namespace NapCatSharp.Mod.Core;

public abstract class Mod
{
    public string ModName => this.GetType().Name;
    public virtual void MetaLifecycle(Lifecycle lifecycle){}
    public virtual void MetaHeartbeat(Heartbeat heartbeat){}
    public virtual void MessagePrivate(PrivateMessage message){}
    public virtual void MessageGroup(GroupMessage message){}
    public virtual void RequestFriend(AddFriend request){}
    public virtual void RequestGroup(AddGroup request){}
    public virtual void MessageSentSelf(Self self){}
}