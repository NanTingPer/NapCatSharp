using NapCatSharp.Mod.Controllers;
using NapCatSharp.Mod.Services;
using System.Text.Json;

namespace NapCatSharp.Mod.Middlewares;

public class JWTMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var jwt = new JWTAttribute();
        if(!await jwt.OnAuthorizationAsync(context)) {
            if(!context.Response.HasStarted) {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(JsonSerializer.Serialize(NotFoundJson.Not));
            }
            return;
        }
        await next.Invoke(context);
    }
}


