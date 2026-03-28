namespace NapCatSharp.Mod.Core;

public static class ModLoader
{
    /// <summary>
    /// 加载全部模组
    /// </summary>
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

    /// <summary>
    /// 加载给定模组
    /// </summary>
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
    /// 获取全部本地可启用模组
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

    /// <summary>
    /// 禁用给定模组
    /// </summary>
    internal static void DisableMod(string modName)
    {
        ModContext.UnLoadMod(modName);
    }

    /// <summary>
    /// 将数据覆盖写入到模组文件中
    /// </summary>
    internal static async Task OverrideModFileAsync(string modName, string fileName, byte[] bytes)
    {
        var tarFileName = FileCreate(modName, fileName);
        var fileStream = File.Create(tarFileName);
        await fileStream.WriteAsync(bytes);
        fileStream.Dispose();
    }

    /// <summary>
    /// 将数据追加到模组文件中
    /// </summary>
    internal static async Task AppendModFileAsync(string modName, string fileName, byte[] bytes)
    {
        var tarFileName = FileCreate(modName, fileName);
        var fileWrite = File.OpenWrite(tarFileName);
        await fileWrite.WriteAsync(bytes);
        fileWrite.Dispose();
    }

    /// <summary>
    /// 删除给定模组的全部文件 含文件夹
    /// <param name="modName">模组名称</param>
    /// <param name="recursive"> 是否递归删除 </param>
    /// </summary>
    internal static void DeleteModFiles(string modName, bool recursive = true)
    {
        var modDirName = Path.Combine(ModContext.ModPath, modName);
        Directory.Delete(modDirName, recursive);
    }

    /// <summary>
    /// 创建给定的文件，Mods/<paramref name="modName"/>/<paramref name="fileName"/>
    /// </summary>
    /// <param name="modName">模组名称</param>
    /// <param name="fileName">文件名称</param>
    /// <returns></returns>
    internal static string FileCreate(string modName, string fileName)
    {
        var modDirName = Path.Combine(ModContext.ModPath, modName);
        var tarFileName = Path.Combine(modDirName, fileName);
        if (!Directory.Exists(modDirName)) {
            Directory.CreateDirectory(modDirName);
        }
        if (!File.Exists(tarFileName)) {
            File.Create(tarFileName).Dispose();
        }
        return tarFileName;
    }
}