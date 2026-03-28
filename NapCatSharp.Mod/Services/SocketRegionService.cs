using NapCatSharp.Core;
using NapCatSharp.Mod.Core;
using System.Collections.Concurrent;

namespace NapCatSharp.Mod.Services;

/// <summary>
/// <see cref="NapCatHttpSocket"/> 注册服务
/// </summary>
public class SocketRegionService : BackgroundService
{
    private ModManager modManager;
    public SocketRegionService(ModManager modManager)
    {
        this.modManager = modManager;
    }
    private readonly BlockingCollection<QueueSocketTask> RegionQueue = [];
    //private readonly BlockingCollection<>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested) {
            var task = RegionQueue.Take(stoppingToken);
            try {
                await task.Socket.Connection(stoppingToken);
            } catch(Exception e) {
                if (!stoppingToken.IsCancellationRequested) {
                    await (task.ConnectionErrorCall?.Invoke(this, task.Socket, e) ?? Task.CompletedTask);
                }
                continue;
            }
            var socket = task.Socket;
            SubscribeEvents(socket);
            Recive(task, stoppingToken);
        }
    }

    /// <summary>
    /// 向队列中添加连接任务
    /// </summary>
    public void Enqueue(QueueSocketTask queueSocket)
    {
        try {
            RegionQueue.TryAdd(queueSocket);
        } catch{}
    }

    public void ReConnection(QueueSocketTask task)
    {
        var socket = task.Socket;
        UnsubscribeEvents(socket);
        Enqueue(task);
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

    private async void Recive(QueueSocketTask task, CancellationToken cancellationToken)
    {
        try {
            await task.Socket.Receive(cancellationToken);
        } catch(Exception e) {
            if (!cancellationToken.IsCancellationRequested) {
                await (task.ReciveErrorCall?.Invoke(this, task.Socket, e) ?? Task.CompletedTask);
            }
        } finally {
            UnsubscribeEvents(task.Socket);
        }
    }
}

public class QueueSocketTask(
    NapCatHttpSocket socket,
    ErrorCall? connectionErrorCall = null,
    ErrorCall? reciveErrorCall = null
    )
{
    public NapCatHttpSocket Socket { get; set; } = socket;
    public ErrorCall? ConnectionErrorCall { get; set; } = connectionErrorCall;
    public ErrorCall? ReciveErrorCall { get; set; } = reciveErrorCall;
}

public delegate Task ErrorCall(SocketRegionService service, NapCatHttpSocket socket, Exception exception);