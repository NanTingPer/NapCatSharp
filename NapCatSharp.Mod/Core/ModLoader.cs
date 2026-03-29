using NapCatSharp.Mod.Services;
using Serilog.Core;
using System.Text;
using System.Text.Json;

namespace NapCatSharp.Mod.Core;

public static class ModLoader
{
    public static SystemLogger logger = null!;
    public static string ModEnableConfigPath => Path.Combine(ModContext.ModPath, "enable.json");
    private readonly static Lock modenablelock = new();
    /// <summary>
    /// 模组启用列表
    /// </summary>
    public static List<string> EnableModList
    {
        get
        {
            string value;
            lock (modenablelock) {
                try {
                    if (!File.Exists(ModEnableConfigPath)) {
                        File.WriteAllText(ModEnableConfigPath, "[]");
                    }
                    value = File.ReadAllText(ModEnableConfigPath, Encoding.UTF8);
                } finally{
                }
            }
            return JsonSerializer.Deserialize<List<string>>(value) ?? [];
        }
        
    }
    
    /// <summary>
    /// 写入模组启用列表
    /// </summary>
    internal static void WriteEnableModList(List<string> enableList)
    {
        lock (modenablelock) {
            try {
                File.WriteAllText(ModEnableConfigPath, JsonSerializer.Serialize(enableList));
            } finally {
            }
        }
    }

    /// <summary>
    /// 加载全部模组
    /// </summary>
    internal static void LoadMods()
    {
        // 仅包含名称
        var validModNames = EnableModList.Intersect(LocalMods());
        foreach (var item in validModNames) {
            LoadMod(item);
        }
        //foreach (var modName in validModNames) {
        //    logger.Info($"加载模组: {modName}");
        //    var context = new ModContext(modName);
        //    var modAssembly = context.LoadFromAssemblyPath(context.assemblyPath);
        //    var modTypes = modAssembly.GetModTypes();
        //    if(modTypes.Length != 1) {
        //        context.Unload();
        //        logger.Error($"模组加载失败: {modName}, 一个程序集只能包含一个Mod，但在{modName}中发现多个");
        //        throw new Exception($"一个程序集只能包含一个Mod，但在{modName}中发现多个");
        //    }
        //    logger.Info($"创建实例: {modName}");
        //    var mod = (Mod)Activator.CreateInstance(modTypes[0])!;
        //    ModContext.Mods.Add(mod);
        //}
    }

    /// <summary>
    /// 加载给定模组
    /// </summary>
    internal static bool LoadMod(string modName)
    {
        if(ModContext.Mods.Any(f => modName.Equals(f.ModName)))
            return true;
        logger.Info($"加载模组: {modName}");
        var moddir = Path.Combine(ModContext.ModPath, modName);
        if (!Directory.Exists(moddir)) {
            logger.Error($"模组文件夹不存在: {modName}");
            return false; // 给定模组文件夹不存在
        }

        var files = Directory.GetFiles(moddir);
        if(!files.Any(file => Path.GetFileName(file).Replace(".dll", "") == modName)) {
            logger.Error($"模组文件夹中找不到主程序集: {moddir}/{modName}");
            return false; // 给定模组文件夹中找不到主程序集 (要与Mod名称一致)
        }

        var context = new ModContext(modName);
        var modAssembly = context.LoadFromAssemblyPath(context.assemblyPath);
        var modTypes = modAssembly.GetModTypes();
        if (modTypes.Length != 1) {
            context.Unload();
            logger.Error($"模组加载失败: {modName}, 一个程序集只能包含一个Mod，但在{modName}中发现多个");
            throw new Exception($"一个程序集只能包含一个Mod，但在{modName}中发现多个");
        }
        var mod = (Mod)Activator.CreateInstance(modTypes[0])!;
        logger.Info($"创建实例: {modName}");
        ModContext.Mods.Add(mod);
        var newlist = EnableModList.ToHashSet();
        newlist.Add(mod.ModName);
        WriteEnableModList([.. newlist]);
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
            logger.Info($"重新加载模组: {modName}");
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
        logger.Info($"禁用模组: {modName}");
        ModContext.UnLoadMod(modName);
        var newlist = EnableModList.ToHashSet();
        newlist.Remove(modName);
        WriteEnableModList([..  newlist]);
    }

    /// <summary>
    /// 将数据覆盖写入到模组文件中
    /// </summary>
    internal static async Task OverrideModFileAsync(string modName, string fileName, byte[] bytes)
    {
        logger.Info($"覆盖模组文件: {modName} {fileName}");
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
        logger.Info($"追加模组文件: {modName} {fileName}");
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
        logger.Info($"删除模组: {modName} ");
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
        logger.Info($"删除模组文件: {modName} {fileName}");
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
        logger.Info($"创建模组文件: {modName} {fileName}");
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
        logger.Info($"获取文件大小: {modName} {fileName}");
        var tarFileName = Path.Combine(ModContext.ModPath, modName, fileName);
        var fileInfo = new FileInfo(tarFileName);
        return fileInfo.Length;
    }
}