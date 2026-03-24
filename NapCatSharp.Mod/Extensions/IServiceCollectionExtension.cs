using System.Reflection;

namespace NapCatSharp.Mod.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddModSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(options => {
            var envName = Assembly.GetExecutingAssembly().GetName().Name;
            var path = Path.Combine(AppContext.BaseDirectory, envName + ".xml");
            options.IncludeXmlComments(path);
        });

        return services;
    }
}
