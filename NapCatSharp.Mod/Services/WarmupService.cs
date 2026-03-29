using NapCatSharp.Mod.Core;

namespace NapCatSharp.Mod.Services;

public class WarmupService(NapCatSocketManager socket) : IHostedService
{
    public NapCatSocketManager socketManager { get; set; } = socket;
    public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
