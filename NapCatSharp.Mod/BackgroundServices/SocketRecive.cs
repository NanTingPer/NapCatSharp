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
        /*
         * 1. 默认的SocketRecive，在启动时获取socket配置。若有 则链接
         * 2. 若未获取到socket配置，则不进行操作 此BackgroundService退出
         * 3. 用户在Web页面添加Socket配置
         */
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
        /*
         * 1. 连接以及接收不应当置于此处
         * 2. 创建全局Socket管理器，统一管理
         * 3. 创建全局消息队列，在此处阻塞循环处理消息，并下发到ModManager中
         * 4. 由Socket管理器将消息追加到全局消息队列中
         */
        await napcatHttpSocket.Connection(stoppingToken);
        await napcatHttpSocket.Receive(stoppingToken);
    }
}
