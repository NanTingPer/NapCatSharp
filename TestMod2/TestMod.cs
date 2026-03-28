using NapCatSharp.EventPushModels.MetaEvents;
using NapCatSharp.Mod.Core;

namespace TestMod2;

public class TestMod2 : Mod
{
    public TestMod2()
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
