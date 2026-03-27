using Microsoft.AspNetCore.Mvc;
using NapCatSharp.Mod.Core;
using NapCatSharp.Mod.Services;

namespace NapCatSharp.Mod.Controllers;

[ApiController]
[Route("modmanager")]
public class ModManagerController(ModManager manager) : ControllerBase
{
    public ModManager manager = manager;
    [HttpPost("modlist")]
    [JWT]
    public ActionResult<IEnumerable<string>> ModList()
    {
        return Ok(manager.Mods.Select(m => m.ModName));
    }

    [HttpPost("disablemod")]
    [JWT]
    public IActionResult DisableMod(string modname)
    {
        var mod = manager.Mods.FirstOrDefault(f => f.ModName == modname);
        if (mod != null) {
            ModLoader.DisableMod(modname);
        }
        return Ok();
    }
}
