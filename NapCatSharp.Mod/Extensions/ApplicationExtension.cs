namespace NapCatSharp.Mod.Extensions;

public static class ApplicationExtension
{
    public static WebApplication UseModSwagger(this WebApplication application)
    {
        application.UseSwagger();
        application.UseSwaggerUI();
        return application;
    }
}
