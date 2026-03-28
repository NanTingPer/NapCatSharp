using NapCatSharp.EventPushModels.MessageEvents;
using NapCatSharp.EventPushModels.MessageSentEvents;
using NapCatSharp.EventPushModels.MetaEvents;
using NapCatSharp.EventPushModels.RequestEvents;
using static NapCatSharp.Core.NapCatHttpSocket;

namespace NapCatSharp.Mod.Core;

public class ModManager
{
    public List<WeakReference<NapCatSharp.Core.Mod>> Mods { get; set; }
    public ModManager(List<WeakReference<NapCatSharp.Core.Mod>> mods)
    {
        Mods = mods;
    }
    public void MetaLifecycle(EventMessageData eventBaseModel)
    {
        if (eventBaseModel.EventData is not Lifecycle lifecycle) return;
        for (int i = 0; i < Mods.Count; i++) {
            if (Mods[i].TryGetTarget(out var target)) target.MetaLifecycle(lifecycle);
        }
    }
    public void MetaHeartbeat(EventMessageData eventBaseModel)
    {
        if (eventBaseModel.EventData is not Heartbeat heartbeat) return;
        for (int i = 0; i < Mods.Count; i++) {
            if (Mods[i].TryGetTarget(out var target)) target.MetaHeartbeat(heartbeat);
        }
    }
    public void MessagePrivate(EventMessageData eventBaseModel)
    {
        if (eventBaseModel.EventData is not PrivateMessage message) return;
        for (int i = 0; i < Mods.Count; i++) {
            if (Mods[i].TryGetTarget(out var target)) target.MessagePrivate(message);
        }
    }
    public void MessageGroup(EventMessageData eventBaseModel)
    {
        if (eventBaseModel.EventData is not GroupMessage message) return;
        for (int i = 0; i < Mods.Count; i++) {
            if (Mods[i].TryGetTarget(out var target)) target.MessageGroup(message);
        }
    }
    public void RequestFriend(EventMessageData eventBaseModel)
    {
        if (eventBaseModel.EventData is not AddFriend request) return;
        for (int i = 0; i < Mods.Count; i++) {
            if (Mods[i].TryGetTarget(out var target)) target.RequestFriend(request);
        }
    }
    public void RequestGroup(EventMessageData eventBaseModel)
    {
        if (eventBaseModel.EventData is not AddGroup request) return;
        for (int i = 0; i < Mods.Count; i++) {
            if (Mods[i].TryGetTarget(out var target)) target.RequestGroup(request);
        }
    }
    public void MessageSentSelf(EventMessageData eventBaseModel)
    {
        if (eventBaseModel.EventData is not Self self) return;
        for (int i = 0; i < Mods.Count; i++) {
            if (Mods[i].TryGetTarget(out var target)) target.MessageSentSelf(self);
        }
    }
}