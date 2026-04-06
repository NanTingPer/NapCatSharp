using NapCatSharp.Mod.Core.ModTypes;

namespace TestMod2;

public class TestModConfigTwo : ModConfig
{
    public string ConfigV { get; set; } = "tywq";
    public TestMCW WCMT { get; set; } = new TestMCW();
}

public class TestMCW
{
    public string TName { get; set; } = "TestMCW";
}
