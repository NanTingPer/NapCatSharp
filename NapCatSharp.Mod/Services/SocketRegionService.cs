using NapCatSharp.Core;
using NapCatSharp.Mod.ConfigEntitys;
using NapCatSharp.Mod.Core;

namespace NapCatSharp.Mod.Services;

/// <summary>
/// <see cref="NapCatHttpSocket"/> 注册服务 <br/>
/// 每个 socket 拥有独立的后台任务，支持无限重试和 CancellationToken 取消。
/// </summary>
public class SocketRegionService : BackgroundService
{
    private readonly ModManager modManager;
    private readonly SystemLogger logger;

    public SocketRegionService(ModManager modManager, SystemLogger logger)
    {
        this.modManager = modManager;
        this.logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // 服务本身不再维护队列，启动即返回。
        // 每个 socket 的生命周期由 StartSocket / StopSocket 独立管理。
        return Task.CompletedTask;
    }

    /// <summary>
    /// 为给定 socket 启动独立的后台连接 + 消息接收任务。
    /// 连接失败后会无限重试，直到 <paramref name="cts"/> 被取消。
    /// </summary>
    /// <param name="name">socket 名称（用于日志）</param>
    /// <param name="socket">socket 实例</param>
    /// <param name="cts">可取消的 CancellationTokenSource</param>
    /// <param name="statusChanged">状态变更回调 (name, newStatus, retryCount)</param>
    public void StartSocket(
        string name,
        NapCatHttpSocket socket,
        CancellationTokenSource cts,
        Action<string, ConnectionStatus, int>? statusChanged = null)
    {
        Task.Run(() => SocketLifecycleAsync(name, socket, cts, statusChanged));
    }

    /// <summary>
    /// 单个 socket 的完整生命周期：连接 -> 接收消息 -> 断开后重试
    /// </summary>
    private async Task SocketLifecycleAsync(
        string name,
        NapCatHttpSocket socket,
        CancellationTokenSource cts,
        Action<string, ConnectionStatus, int>? statusChanged)
    {
        var token = cts.Token;
        int retryCount = 0;

        while (!token.IsCancellationRequested) {
            try {
                // 设置状态为"连接中"
                statusChanged?.Invoke(name, ConnectionStatus.Connecting, retryCount);
                logger.Info($"Socket [{name}] 正在连接 {socket.Uri} (重试次数: {retryCount})");

                await socket.Connection(token);

                // 连接成功，订阅事件
                SubscribeEvents(socket);
                statusChanged?.Invoke(name, ConnectionStatus.Connected, 0);
                logger.Info($"Socket [{name}] 连接成功 {socket.Uri}");

                retryCount = 0;

                // 开始接收消息（此方法会一直阻塞直到 token 取消或连接断开）
                await socket.Receive(token);
            } catch (OperationCanceledException) {
                // 正常取消，退出循环
                logger.Info($"Socket [{name}] 操作被取消");
                break;
            } catch (Exception e) {
                if (token.IsCancellationRequested) break;

                logger.Error($"Socket [{name}] 错误: {e.Message}");
            } finally {
                UnsubscribeEvents(socket);
            }

            if (token.IsCancellationRequested) break;

            // 需要重试
            retryCount++;
            statusChanged?.Invoke(name, ConnectionStatus.Connecting, retryCount);
            logger.Info($"Socket [{name}] 断开连接，准备重试... (第 {retryCount} 次)");

            // 指数退避: 1s, 2s, 4s, 8s, 16s, 30s(上限)
            var delay = TimeSpan.FromSeconds(Math.Min(Math.Pow(2, Math.Min(retryCount, 5)), 30));
            try {
                await Task.Delay(delay, token);
            } catch (OperationCanceledException) {
                break;
            }
        }

        // 最终状态
        if (!token.IsCancellationRequested) {
            statusChanged?.Invoke(name, ConnectionStatus.Disconnected, retryCount);
        }
        logger.Info($"Socket [{name}] 生命周期结束");
    }

    private void UnsubscribeEvents(NapCatHttpSocket socket)
    {
        socket.MessageGroup -= modManager.MessageGroup;
        socket.MessagePrivate -= modManager.MessagePrivate;
        socket.MessageSentSelf -= modManager.MessageSentSelf;
        socket.RequestGroup -= modManager.RequestGroup;
        socket.RequestFriend -= modManager.RequestFriend;
        socket.MetaHeartbeat -= modManager.MetaHeartbeat;
        socket.MetaLifecycle -= modManager.MetaLifecycle;
    }

    private void SubscribeEvents(NapCatHttpSocket socket)
    {
        socket.MessageGroup += modManager.MessageGroup;
        socket.MessagePrivate += modManager.MessagePrivate;
        socket.MessageSentSelf += modManager.MessageSentSelf;
        socket.RequestGroup += modManager.RequestGroup;
        socket.RequestFriend += modManager.RequestFriend;
        socket.MetaHeartbeat += modManager.MetaHeartbeat;
        socket.MetaLifecycle += modManager.MetaLifecycle;
    }
}