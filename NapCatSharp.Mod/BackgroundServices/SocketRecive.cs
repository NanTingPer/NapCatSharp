using NapCatSharp.Core;
using NapCatSharp.Mod.Core;

namespace NapCatSharp.Mod.BackgroundServices;

public class SocketRecive : BackgroundService
{
    public const string socketUriConfig = "socketUri";
    public const string socketPasswordConfig = "socketPassword";
    private readonly NapCatHttpSocket napcatHttpSocket;
    public SocketRecive(ModManager modManager, IConfiguration configuration)
    {
        var socketUrl = configuration.GetValue<string?>(socketUriConfig, null);
        var socketPassword = configuration.GetValue<string?>(socketPasswordConfig, null);
        if (string.IsNullOrWhiteSpace(socketUrl) || string.IsNullOrWhiteSpace(socketPassword)) {
            configuration[socketUriConfig] = "";
            configuration[socketPasswordConfig] = "";
            throw new Exception("socketUri或socketPassword未配置");
        }
        napcatHttpSocket = new NapCatHttpSocket()
        {
            Password = socketPassword,
            Uri = new Uri(socketUrl)
        };
        napcatHttpSocket.MessageGroup += modManager.MessageGroup;
        napcatHttpSocket.MessagePrivate += modManager.MessagePrivate;
        napcatHttpSocket.MessageSentSelf += modManager.MessageSentSelf;
        napcatHttpSocket.RequestGroup += modManager.RequestGroup;
        napcatHttpSocket.RequestFriend += modManager.RequestFriend;
        napcatHttpSocket.MetaHeartbeat += modManager.MetaHeartbeat;
        napcatHttpSocket.MetaLifecycle += modManager.MetaLifecycle;

    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await napcatHttpSocket.Connection(stoppingToken);
        await napcatHttpSocket.Receive(stoppingToken);
    }
}
