using NapCatSharp.Mod.Core.ModTypes;

namespace TestMod2;

public class TestModConfig : ModConfig
{
    public string 测试配置项 { get; set; } = "你好测试配置项目";
    public List<string> 测试配置项集合 { get; set; } = ["哈哈", "哈韩"];
}