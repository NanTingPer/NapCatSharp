using NapCatSharp.OB11.OB11MessageModels;

namespace NapCatSharp.Mod.Core;

public static class ModLoader
{
    internal static void LoadMods()
    {
        // 仅包含名称
        var validModNames = LocalMods();

        foreach (var modName in validModNames) {
            var context = new ModContext(modName);
            var modAssembly = context.LoadFromAssemblyPath(context.assemblyPath);
            var modTypes = modAssembly.GetModTypes();
            if(modTypes.Length != 1) {
                context.Unload();
                throw new Exception($"一个程序集只能包含一个Mod，但在{modName}中发现多个");
            }
            var mod = (NapCatSharp.Core.Mod)Activator.CreateInstance(modTypes[0])!;
            ModContext.Mods.Add(mod);
        }
    }

    internal static bool LoadMod(string modName)
    {
        var moddir = Path.Combine(ModContext.ModPath, modName);
        if (!Directory.Exists(moddir)) {
            return false; // 给定模组文件夹不存在
        }

        var files = Directory.GetFiles(moddir);
        if(!files.Any(file => Path.GetFileName(file).Replace(".dll", "") == modName)) {
            return false; // 给定模组文件夹中找不到主程序集 (要与Mod名称一致)
        }

        var context = new ModContext(modName);
        var modAssembly = context.LoadFromAssemblyPath(context.assemblyPath);
        var modTypes = modAssembly.GetModTypes();
        if (modTypes.Length != 1) {
            context.Unload();
            throw new Exception($"一个程序集只能包含一个Mod，但在{modName}中发现多个");
        }
        var mod = (NapCatSharp.Core.Mod)Activator.CreateInstance(modTypes[0])!;
        ModContext.Mods.Add(mod);
        return true;
    }

    /// <summary>
    /// 本地可启用模组
    /// </summary>
    internal static List<string> LocalMods()
    {
        var modPath = ModContext.ModPath;
        if (!Directory.Exists(modPath)) {
            Directory.CreateDirectory(modPath);
        }
        var modNames = Directory.GetDirectories(modPath); // 绝对文件夹路径

        // 仅包含名称
        var validModNames = modNames.Where(dir => {
            var modName = dir.Split(Path.DirectorySeparatorChar)[^1];
            var files = Directory.GetFiles(dir);
            foreach (var file in files) {
                if (Path.GetFileName(file).Replace(".dll", "") == modName) {
                    return true;
                }
            }
            return false;
        }).Select(f => f.Split(Path.DirectorySeparatorChar)[^1]);
        return [.. validModNames];
    }

    internal static bool ReLoadMod(string modName)
    {
        try {
            ModContext.UnLoadMod(modName);
            LoadMod(modName);
        } catch {
            return false;
        }
        return true;
    }

    internal static void DisableMod(string modName)
    {
        ModContext.UnLoadMod(modName);
    }
}