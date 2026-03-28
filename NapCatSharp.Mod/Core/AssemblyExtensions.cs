using System.Reflection;

namespace NapCatSharp.Mod.Core;

public static class AssemblyExtensions
{
    public static Type[] GetModTypes(this Assembly assembly)
    {
        return assembly.GetTypes().Where(f => f.IsAssignableTo(typeof(Mod))
            && !f.IsAbstract && !f.IsInterface).ToArray();
    }
}
