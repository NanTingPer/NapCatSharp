using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace NapCatSharp.Mod.Services;

[AttributeUsage(AttributeTargets.Method)]
public class JWTAttribute : Attribute
{
    private readonly static JsonWebTokenHandler handler = new JsonWebTokenHandler();
    public async Task<bool> OnAuthorizationAsync(HttpContext/*AuthorizationFilterContext*/ context)
    {
#if DEBUG
        //return true;
#endif
        // view 没有终结点
#pragma warning disable CS0162 // 检测到无法访问的代码
        var actionDesc = context.GetEndpoint()?.Metadata?.GetMetadata<JWTAttribute>();
        var le = context.GetEndpoint();
#pragma warning restore CS0162 // 检测到无法访问的代码
        if (actionDesc == null) return true;
        //if (actionDesc?.MethodInfo.GetCustomAttribute<JWTAttribute>() == null) {
        //    return true;
        //}

        var jwtStr = context.Request.Headers.Authorization.ToString();
        if(jwtStr.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)) {
            jwtStr = jwtStr["Bearer ".Length..];
        }
        jwtStr = jwtStr.Trim();
        var configuration = context.RequestServices.GetService<IConfiguration>();
        var skey = configuration!.GetValue("jwtKey", string.Empty);
        if (!await VerifyToken(jwtStr, skey)) {
            //context.Request = new NotFoundResult();
            return false;
        }
        //context.User.AddIdentity(new ClaimsIdentity([new Claim("")]))
        return true;
    }

    /// <summary>
    /// 验证jwt
    /// </summary>
    /// <param name="jwtStr"> jwt字符串 </param>
    /// <param name="skey"> 密钥 </param>
    /// <returns></returns>
    private static async Task<bool> VerifyToken(string jwtStr, string skey)
    {
        if (string.IsNullOrEmpty(jwtStr) || string.IsNullOrEmpty(skey)) {
            return false;
        }

        var securityKeyBytes = Encoding.UTF8.GetBytes(skey);
        var parameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(securityKeyBytes),
            ClockSkew = TimeSpan.Zero
        };
        try {
            return (await handler.ValidateTokenAsync(jwtStr, parameters)).IsValid;
        } catch /*(Exception e)*/ {
            return false;
        }
    }

    public static string CreateToken(string key)
    {
        var siKey = Encoding.UTF8.GetBytes(key);
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(siKey), SecurityAlgorithms.HmacSha256Signature);
        var jwtTokenDesc = new SecurityTokenDescriptor()
        {
            SigningCredentials = signingCredentials,
            Expires = DateTime.UtcNow.AddMinutes(10), //todo 测试2秒
            NotBefore = DateTime.UtcNow
        };
        return handler.CreateToken(jwtTokenDesc);
    }
}
