using NapCatSharp.Mod.BackgroundServices;
using NapCatSharp.Mod.Core;
using NapCatSharp.Mod.Services;
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

    public static IServiceCollection AddNapCatSharpServices(this IServiceCollection services)
    {
        ModLoader.LoadMods();
        var modManager = new ModManager(ModContext.Mods);
            services
            .AddSingleton(ModContext.Mods)
            .AddSingleton<ModManager>(modManager)
            .AddSingleton<SocketRegionService>()
            .AddHostedService<SocketRegionService>(sp =>
                sp.GetService<SocketRegionService>()!)
            .AddSingleton<NapCatSocketManager>()
            .AddHostedService<WarmupService>()
            ;
            //.AddHostedService<SocketRecive>()
        return services;
    }
}
