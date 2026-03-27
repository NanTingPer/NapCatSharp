using Microsoft.AspNetCore.Mvc;
using NapCatSharp.Mod.Core;
using NapCatSharp.Mod.Services;
using System.Text.Json.Serialization;

namespace NapCatSharp.Mod.Controllers;

[ApiController]
[Route("modmanager")]
public class ModManagerController(ModManager manager) : ControllerBase
{
    public ModManager manager = manager;
    [JWT]
    [HttpPost("modlist")]
    public ActionResult<IEnumerable<string>> ModList()
    {
        return Ok(manager.Mods.Select(m => m.ModName));
    }

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

    [JWT]
    [HttpPost("localMods")]
    public IActionResult LocalMods()
    {
        return Ok(ModLoader.LocalMods().Except(manager.Mods.Select(f => f.ModName)));
    }

    [JWT]
    [HttpPost("reloadmod")]
    public IActionResult ReLoadMod([FromBody] SimpleModInput input)
    {
        ModLoader.ReLoadMod(input.ModName);
        return Ok();
    }

    [JWT]
    [HttpPost("enablemod")]
    public IActionResult LoadMod([FromBody] SimpleModInput input)
    {
        ModLoader.LoadMod(input);
        return Ok();
    }
}

public class SimpleModInput
{
    [JsonPropertyName("modname")]
    public string ModName { get; set; } = string.Empty;
    public static implicit operator string(SimpleModInput input) => input.ModName;
}