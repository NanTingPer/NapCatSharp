namespace NapCatSharp.Mod.Core.ModTypes;

public abstract class ModType
{
    protected ModType()
    {
        Load();
    }
    public virtual void Unload()
    {

    }

    public virtual void Load()
    {

    }

    /// <summary>
    /// 获取配置
    /// </summary>
    public T? GetConfig<T>()
        where T : ModConfig
    {
        return ModConfigManager.GetConfig<T>();
    }
}
