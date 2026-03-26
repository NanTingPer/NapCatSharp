using Microsoft.AspNetCore.Mvc;
using NapCatSharp.Mod.Core;

namespace NapCatSharp.Mod.Controllers;

#if DEBUG
[ApiController]
[Route("socket")]
public class SocketController(NapCatSocketManager manager) : ControllerBase
{
    private NapCatSocketManager manager = manager;

    /// <summary>
    /// 创建<see cref="NapCatSharp.Core.NapCatHttpSocket"/>
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("create")]
    public IActionResult CreateSocket([FromBody] CreateSocketModel model)
    {
        Uri? uri = null;
        try {
            uri = new Uri(model.Uri);
        } catch(Exception e) {
            return BadRequest(e.Message);
        }
        if(manager.CreateSocket(model.Name, uri, model.Password, out var exp)) {
            return Ok();
        }
        return BadRequest(exp.Message);
    }
}

public class CreateSocketModel
{
    /// <summary>
    /// 链接名称
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// 链接Url 例: ws://127.0.0.1:6666
    /// </summary>
    public string Uri { get; set; } = string.Empty;
    /// <summary>
    /// 链接密钥
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
#endif