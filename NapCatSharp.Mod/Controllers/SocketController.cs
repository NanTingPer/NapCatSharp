using Microsoft.AspNetCore.Mvc;
using NapCatSharp.Mod.ConfigEntitys;
using NapCatSharp.Mod.Core;
using NapCatSharp.Mod.Services;

namespace NapCatSharp.Mod.Controllers;

[ApiController]
[Route("socket")]
public class SocketController(NapCatSocketManager manager, IConfiguration configuration) : ControllerBase
{
    private readonly IConfiguration configuration = configuration;
    private readonly NapCatSocketManager manager = manager;

    /// <summary>
    /// 创建<see cref="NapCatSharp.Core.NapCatHttpSocket"/>
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [JWT]
    [HttpPost("create")]
    public IActionResult CreateSocket([FromBody] CreateSocketInput model)
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

    /// <summary>
    /// 获取当前已启用的socket列表
    /// </summary>
    /// <returns></returns>
    [JWT]
    [HttpPost("socketList")]
    public ActionResult<List<SocketEntity>> SocketList()
    {
        return Ok(manager.Sockets);
    }
}

public class CreateSocketInput
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