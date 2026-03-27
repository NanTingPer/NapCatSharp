using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NapCatSharp.Mod.Models;

namespace NapCatSharp.Mod.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult ModManager()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

[ApiController]
[Route("api")]
public class ApiController : ControllerBase
{
    /// <summary>
    /// 返回Ok 用于测试swagger
    /// </summary>
    /// <returns></returns>
    [HttpPost("retOk")]
    public IActionResult OKEd()
    {
        return Ok();
    }
}
