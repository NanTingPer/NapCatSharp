using NapCatSharp.Mod.Core.ModTypes;
using System.Reflection;

namespace NapCatSharp.Mod.Extensions;

public static class AssemblyExtensions
{
    /// <summary>
    /// 获取实现了<see cref="NapCatSharp.Mod.Core.ModTypes.Mod"/>的类型
    /// </summary>
    public static Type[] GetModTypes(this Assembly assembly)
    {
        return [.. assembly.GetTypes().Where(f => f.IsAssignableTo(typeof(NapCatSharp.Mod.Core.ModTypes.Mod))
            && !f.IsAbstract && !f.IsInterface)];
    }

    /// <summary>
    /// 获取实现了<see cref="ModConfig"/>的类型
    /// </summary>
    public static Type[] GetConfigType(this Assembly assembly)
    {
        return [.. assembly.GetTypes().Where(f => f.IsAssignableTo(typeof(ModConfig))
            && !f.IsAbstract && !f.IsInterface)];
    }

    /// <summary>
    /// 获取实现了<typeparamref name="TReablize"/>的类型
    /// </summary>
    public static Type[] GetRealizeType<TReablize>(this Assembly assembly)
    {
        return [.. assembly.GetTypes().Where(f => f.IsAssignableTo(typeof(TReablize))
            && !f.IsAbstract && !f.IsInterface)];
    }
}
