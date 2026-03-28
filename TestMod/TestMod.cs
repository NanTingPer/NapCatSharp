using NapCatSharp.EventPushModels.MetaEvents;
using NapCatSharp.Mod.Core;

namespace TestMod;

public class TestMod : Mod
{
    public TestMod()
    {
        _ = 1;
    }

    public override void MetaHeartbeat(Heartbeat heartbeat)
    {
        _ = 1;
        base.MetaHeartbeat(heartbeat);
    }

    public override void MetaLifecycle(Lifecycle lifecycle)
    {
        _ = 1;
        base.MetaLifecycle(lifecycle);
    }
}
