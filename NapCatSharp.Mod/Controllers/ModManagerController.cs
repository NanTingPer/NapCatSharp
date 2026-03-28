using Microsoft.AspNetCore.Mvc;
using NapCatSharp.Mod.Core;
using NapCatSharp.Mod.Extensions;
using NapCatSharp.Mod.Services;
using System.Text.Json.Serialization;

namespace NapCatSharp.Mod.Controllers;

[ApiController]
[Route("modmanager")]
public class ModManagerController(ModManager manager) : ControllerBase
{
    public ModManager manager = manager;

    /// <summary>
    /// 获取已经启用的模组列表
    /// </summary>
    [JWT]
    [HttpPost("modlist")]
    public ActionResult<IEnumerable<string>> ModList()
    {
        return Ok(manager.Mods.Select(m => m.ModName).Where(m => m != string.Empty));
    }

    /// <summary>
    /// 禁用给定模组
    /// </summary>
    /// <param name="modname"> 要被禁用的模组名称 </param>
    /// <returns></returns>
    [JWT]
    [HttpPost("disablemod")]
    public IActionResult DisableMod([FromBody] SimpleModInput modname)
    {
        var mod = manager.Mods.FirstOrDefault(f => f.ModName == modname);
        if (mod != null) {
            ModLoader.DisableMod(modname);
        }
        return Ok();
    }

    /// <summary>
    /// 获取本地模组列表，只显示可启用的，如模组已被启用则不会在此列表中
    /// </summary>
    [JWT]
    [HttpPost("localMods")]
    public IActionResult LocalMods()
    {
        return Ok(ModLoader.LocalMods().Except(manager.Mods.Select(f => f.ModName)));
    }

    /// <summary>
    /// 重新加载给定模组
    /// </summary>
    /// <param name="input"> 要重新加载的模组名称 </param>
    /// <returns></returns>
    [JWT]
    [HttpPost("reloadmod")]
    public IActionResult ReLoadMod([FromBody] SimpleModInput input)
    {
        ModLoader.ReLoadMod(input.ModName);
        return Ok();
    }

    /// <summary>
    /// 启用给定模组
    /// </summary>
    /// <param name="input"> 要启用的模组名称 </param>
    /// <returns></returns>
    [JWT]
    [HttpPost("enablemod")]
    public IActionResult LoadMod([FromBody] SimpleModInput input)
    {
        ModLoader.LoadMod(input);
        return Ok();
    }

    /// <summary>
    /// 删除给定模组名称的实体文件
    /// </summary>
    /// <param name="input"> 要删除的模组名称 </param>
    /// <returns></returns>
    [JWT]
    [HttpPost("deletemodfiles")]
    public IActionResult DeleteModFiles([FromBody] SimpleModInput input)
    {
        ModLoader.DeleteModFiles(input);
        return Ok();
    }

    /// <summary>
    /// 追加内容到模组文件
    /// </summary>
    /// <returns></returns>
    [JWT]
    [HttpPost("appendmodFile")]
    public async Task<IActionResult> AppendModFile([FromBody] AppendModFileInput input)
    {
        (string fileName, string modName, bool isEnd, long fileSize) = (input.FileName, input.ModName, input.IsEnd, input.FileSize);
        if(fileName == string.Empty || modName == string.Empty) {
            return BadRequest(new SimpleControllerResult(){ Code = 400, ErrorMsg = "文件名称或模组名称不能为空" });
        }
        var fileBytes = Convert.FromBase64String(input.Base64);
        await ModLoader.AppendModFileAsync(modName, fileName, fileBytes);
        if (isEnd) { 
            var endFileSize = ModLoader.GetFileSize(modName, fileName);
            if (endFileSize != fileSize) {
                ModLoader.DeleteFile(modName, fileName);
                return BadRequest(new SimpleControllerResult() { Code = 400, ErrorMsg = $"文件终止大小{endFileSize}与目标{fileSize}不一致" });
            }
        }
        return Ok();
    }
}

public class SimpleModInput
{
    [JsonPropertyName("modname")]
    public string ModName { get; set; } = string.Empty;
    public static implicit operator string(SimpleModInput input) => input.ModName;
}

public class AppendModFileInput
{
    /// <summary>
    /// 模组名称
    /// </summary>
    [JsonPropertyName("modName")]
    public string ModName { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小
    /// </summary>
    [JsonPropertyName("fileSize")]
    public long FileSize { get; set; } = 0L;

    /// <summary>
    /// 文件名称
    /// </summary>
    [JsonPropertyName("fileName")]
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 当前片的字节数据
    /// </summary>
    [JsonPropertyName("base64")]
    public string Base64 { get; set; } = string.Empty;

    /// <summary>
    /// 是否结束
    /// </summary>
    [JsonPropertyName("isEnd")]
    public bool IsEnd { get; set; } = false;
}