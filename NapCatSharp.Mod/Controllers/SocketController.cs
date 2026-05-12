using Microsoft.AspNetCore.Mvc;
using NapCatSharp.Mod.ConfigEntitys;
using NapCatSharp.Mod.Core;
using NapCatSharp.Mod.Services;
using System.Text.Json.Serialization;

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
    [JWT]
    [HttpPost("create")]
    public IActionResult CreateSocket([FromBody] SimpleSocketInput model)
    {
        Uri? uri = null;
        try {
            uri = new Uri(model.Uri);
        } catch(Exception e) {
            return BadRequest(e.Message);
        }
        if(manager.Create(model.Name, uri, model.Password, out var exp)) {
            return Ok();
        }
        return BadRequest(exp.Message);
    }

    /// <summary>
    /// 获取当前已启用的socket列表（含连接状态）
    /// </summary>
    [JWT]
    [HttpPost("socketList")]
    public ActionResult<List<SocketStatusResponse>> SocketList()
    {
        var sockets = manager.Sockets.Where(f => f.IsEnable).Select(ToResponse).ToList();
        return Ok(sockets);
    }

    /// <summary>
    /// 被禁用的 socket 列表
    /// </summary>
    [JWT]
    [HttpPost("disableList")]
    public ActionResult<List<SocketStatusResponse>> DisableSocketList()
    {
        var sockets = manager.Sockets.Where(f => f.IsEnable == false).Select(ToResponse).ToList();
        return Ok(sockets);
    }

    /// <summary>
    /// 启用socket
    /// </summary>
    [JWT]
    [HttpPost("enable")]
    public IActionResult Enable([FromBody] NameInput input)
    {
        manager.Enable(input.Name);
        return Ok();
    }

    /// <summary>
    /// 删除socket
    /// </summary>
    [JWT]
    [HttpPost("delete")]
    public IActionResult Delete([FromBody] NameInput input)
    {
        manager.Delete(input.Name);
        return Ok();
    }

    /// <summary>
    /// 禁用socket
    /// </summary>
    [JWT]
    [HttpPost("disable")]
    public IActionResult Disable([FromBody] NameInput input)
    {
        manager.Disable(input.Name);
        return Ok();
    }

    private static SocketStatusResponse ToResponse(SocketEntity entity)
    {
        return new SocketStatusResponse
        {
            Name = entity.Name,
            Uri = entity.Uri,
            Password = entity.Password,
            IsEnable = entity.IsEnable,
            ConnectionStatus = entity.Status switch
            {
                ConnectionStatus.Disconnected => "disconnected",
                ConnectionStatus.Connecting => "connecting",
                ConnectionStatus.Connected => "connected",
                _ => "disconnected"
            },
            RetryCount = entity.RetryCount
        };
    }
}

public class SimpleSocketInput
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

public class NameInput
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

/// <summary>
/// 返回给前端的 Socket 状态响应
/// </summary>
public class SocketStatusResponse
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("uri")]
    public string Uri { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    [JsonPropertyName("isEnable")]
    public bool IsEnable { get; set; } = false;

    /// <summary>
    /// 连接状态: "disconnected" | "connecting" | "connected"
    /// </summary>
    [JsonPropertyName("connectionStatus")]
    public string ConnectionStatus { get; set; } = "disconnected";

    /// <summary>
    /// 当前重试次数
    /// </summary>
    [JsonPropertyName("retryCount")]
    public int RetryCount { get; set; } = 0;
}