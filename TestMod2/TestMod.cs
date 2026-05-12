using NapCatSharp.EventPushModels.MessageEvents;
using NapCatSharp.EventPushModels.MetaEvents;
using NapCatSharp.Mod.Core.ModTypes;

namespace TestMod2;

public class TestConfigOptions: ModConfig
{
    public string Option { get; set; } = "string";
    public List<string> ids { get; set; } = ["av"];
}

public class TestMod2 : Mod
{
    public TestMod2()
    {
        _ = 1;
        var config =  GetConfig<TestModConfig>()!;
        var twoConfig = GetConfig<TestModConfigTwo>()!;
        twoConfig.WCMT = new TestMCW(){ TName = "AAAAAAAAA" };
        twoConfig.WCMT.TName = "BBBBBBBBBBBBBBBBBBBBBBBBBBB";
    }

    public override void MetaHeartbeat(Heartbeat heartbeat)
    {
        _ = 1;
        Console.WriteLine("收到心跳");
        Console.WriteLine("收到心跳2");
        Console.WriteLine("收到心跳3");
        _ = 2;
        base.MetaHeartbeat(heartbeat);
    }

    public override void MetaLifecycle(Lifecycle lifecycle)
    {
        _ = 1;
        base.MetaLifecycle(lifecycle);
    }

    public override void MessageGroup(GroupMessage message)
    {
        _ = 1;
        base.MessageGroup(message);
    }
}
