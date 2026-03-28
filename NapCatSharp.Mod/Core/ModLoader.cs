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
            ModContext.Mods.Add(new WeakReference<NapCatSharp.Core.Mod>(mod));
        }
    }

    /// <summary>
    /// 加载给定模组
    /// </summary>
    internal static bool LoadMod(string modName)
    {
        if(ModContext.Mods.Any(f => {
            if(f.TryGetTarget(out var target)){
                return target.ModName == modName;
            }
            return false;
        }))
            return true; // 已经存在

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
        ModContext.Mods.Add(new WeakReference<NapCatSharp.Core.Mod>(mod));
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
        DisableMod(modName);
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
        DisableMod(modName);
        var tarFileName = FileCreate(modName, fileName);
        var fileWrite = /*File.OpenHandle(tarFileName, FileMode.Append, FileAccess.Write, FileShare.Write);*/File.OpenWrite(tarFileName);
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
        DisableMod(modName);
        var modDirName = Path.Combine(ModContext.ModPath, modName);
        if (Directory.Exists(modDirName)) {
            Directory.Delete(modDirName, recursive);
        }
    }

    /// <summary>
    /// 删除给定模组中的给定文件
    /// </summary>
    internal static void DeleteFile(string modName, string fileName)
    {
        DisableMod(modName);
        var tarFileName = Path.Combine(ModContext.ModPath, modName, fileName);
        var fileInfo = new FileInfo(tarFileName);
        if (fileInfo.Exists) {
            fileInfo.Delete();
        }
    }

    /// <summary>
    /// 创建给定的文件，Mods/<paramref name="modName"/>/<paramref name="fileName"/>
    /// </summary>
    /// <param name="modName">模组名称</param>
    /// <param name="fileName">文件名称</param>
    /// <returns></returns>
    internal static string FileCreate(string modName, string fileName)
    {
        DisableMod(modName);
        var modDirName = Path.Combine(ModContext.ModPath, modName);
        var tarFileName = Path.Combine(modDirName, fileName);
        if (!Directory.Exists(modDirName)) {
            Directory.CreateDirectory(modDirName);
        }
        if (!File.Exists(tarFileName)) {
            using(var fs = File.Create(tarFileName)) {
                _ = fs;
            }
        }
        return tarFileName;
    }

    internal static long GetFileSize(string modName, string fileName)
    {
        var tarFileName = Path.Combine(ModContext.ModPath, modName, fileName);
        var fileInfo = new FileInfo(tarFileName);
        return fileInfo.Length;
    }
}