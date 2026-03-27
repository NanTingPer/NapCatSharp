using Microsoft.AspNetCore.Mvc;
using NapCatSharp.Mod.Services;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace NapCatSharp.Mod.Controllers;

[ApiController]
[Route("login")]
public class LoginController(IConfiguration configuration) : ControllerBase
{
    private readonly IConfiguration configuration = configuration;
    /// <summary> 存放RSA缓存 </summary>
    private readonly static ConcurrentDictionary<string, RSA> requestCache = [];
    /// <summary> 存放RAS清除任务 </summary>
    private readonly static ConcurrentDictionary<string, (Task, CancellationTokenSource)> RSACacheTask = [];
    /// <summary> 获取公钥 </summary>
    [HttpPost("publicKey")]
    public ActionResult<GetPublicKeyOutputModel> GetPublicKey()
    {
        var rsa = RSA.Create(2048);
        var guid = Guid.NewGuid();
        var requestId = guid.ToString("N");
        requestCache[requestId] = rsa; // 32位数字
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var cacheTask = Task.Run(async () => {
            await Task.Delay(TimeSpan.FromMinutes(5), cancellationToken);
            if (cancellationToken.IsCancellationRequested) {
                return; // 释放取消请求，return 因为肯定是被API访问了
            }
            if (requestCache.TryRemove(requestId, out var rsa)) {
                rsa.Dispose();
            }
        }, cancellationToken);
        RSACacheTask[requestId] = (cacheTask, cancellationTokenSource);
        var publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
        return Ok(new GetPublicKeyOutputModel()
        {
            PublicKey = publicKey,
            RequestId = requestId
        });
    }

    [HttpPost("login")]
    public ActionResult<LoginOutputModel> Login([FromBody] LoginInputModel login)
    {
        try {
            if (login.RequestId == string.Empty) return NotFound(NotFoundJson.Not);
            CancellationRSATask(login.RequestId);
            if (!requestCache.TryRemove(login.RequestId, out var rsa)) {
                return NotFound(NotFoundJson.Not);
            }
            var passwordBytes = Convert.FromBase64String(login.Password);
            var decryptBytes = rsa.Decrypt(passwordBytes, RSAEncryptionPadding.Pkcs1/*OaepSHA512*/); //  jsencrypt使用pkcs1
            rsa.Dispose();
            var password = Encoding.UTF8.GetString(decryptBytes);
            if (VerifyPassword(password)) {
                var skey = configuration.GetValue("jwtKey", string.Empty);
                return Ok(new LoginOutputModel() { Token = JWTAttribute.CreateToken(skey) });
            } else {
                return NotFound(NotFoundJson.Not);
            }
        } catch/*(Exception e)*/ {
            return NotFound(NotFoundJson.Not);
        } finally {
            CancellationRSATask(login.RequestId);
            if (requestCache.TryRemove(login.RequestId, out var rsa)) {
                rsa.Dispose();
            }
        }
    }

    [NonAction]
    private bool VerifyPassword(string password)
    {
        //var value = password.Split("qazwsxedcrty");
        try {
            //var pass = value[0];
            //var slat = value[1];
            var locaPass = configuration.GetValue<string>("password", string.Empty);
            //var locaSlat = configuration.GetValue<string>("slat", string.Empty);
            //"salt": ""
            if(password == locaPass) {
                return true;
            }
        } catch {
            return false;
        }
        return false;
    }

    [NonAction]
    private static void CancellationRSATask(string requestId)
    {
        if(RSACacheTask.TryGetValue(requestId, out var value)) {
            value.Item2.Cancel();
        }
    }
}

public class GetPublicKeyOutputModel
{
    [JsonPropertyName("requestId")]
    public string RequestId { get; set; } = string.Empty;
    [JsonPropertyName("publicKey")]
    public string PublicKey { get; set; } = string.Empty;
}

public class LoginInputModel
{
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
    [JsonPropertyName("requestId")]
    public string RequestId { get; set; } = string.Empty;
}

public class LoginOutputModel
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;
}

/// <summary>
/// <see cref="LoginController.RSACacheTask"/> 存放RSA缓存，其中Task是维护的任务，五分钟后使RSA过期。<br />
/// <see cref="LoginController.requestCache"/> 存放请求Id对于的RSA缓存，请求Id与任务缓存一致，可以使用id获取rsa从而获取私钥。<br />
/// 在http中RSA不安全，在https中RSA非必要。 就当给服务器上点压力了。 <br />
/// </summary>
file class Note(){}
