using Microsoft.AspNetCore.Mvc;
using NapCatSharp.Mod.Core;
using NapCatSharp.Mod.Services;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace NapCatSharp.Mod.Controllers;

[ApiController]
[Route("modconfig")]
public class ModConfigManagerController : ControllerBase
{
    [JWT]
    [HttpPost("getconfigs")]
    public ActionResult<List<GetConfigsOutput>> GetConfigs()
    {
        return Ok(ModConfigManager.GetAllConfigs().Select(f => {
            var output = new GetConfigsOutput()
            { 
                ModName = f.modName, 
                Configs = f.Item2.Select(f => {
                    var jo = f.ToJsonObject();
                    jo["configName"] = f.Name();
                    return jo;
                }).ToList()
            };
            return output;
        }));  
    }

    [JWT]
    [HttpPost("updateconfig")]
    public IActionResult UpdateConfig(EditConfigInput input)
    {
        ModConfigManager.UpdateConfig(input.ModName, input.ConfigName, input.Config);
        return Ok();
    }
}

public class GetConfigsOutput
{
    [JsonPropertyName("modName")]
    public string ModName { get; set; } = string.Empty;
    
    [JsonPropertyName("configs")]
    public List<JsonObject> Configs { get; set; } = [];
}

public class EditConfigInput
{
    /// <summary>
    /// 模组名称
    /// </summary>
    [JsonPropertyName("modName")]
    public string ModName { get; set; } = string.Empty;
    /// <summary>
    /// 配置值
    /// </summary>
    [JsonPropertyName("config")]
    public JsonObject Config { get; set; } = [];
    /// <summary>
    /// 配置名称
    /// </summary>
    [JsonPropertyName("configName")]
    public string ConfigName { get; set; } = string.Empty;
}
