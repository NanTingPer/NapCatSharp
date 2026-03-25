namespace NapCatSharp.Mod.Core;

public static class ModLoader
{
    internal static void LoadMods()
    {
        var modPath = ModContext.ModPath;
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
}