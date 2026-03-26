using NapCatSharp.Core;
using NapCatSharp.Mod.ConfigEntitys;
using System.Collections.Concurrent;

namespace NapCatSharp.Mod.Core;

/// <summary>
/// 1. <see cref="NapCatSocketManager"/> 用于在Web / 控制器中创建Socket <br/>
/// - <see cref="NapCatSocketManager.CreateSocket(string, Uri, string, out Exception?)"/> 只负责创建实例，对于链接以及事件订阅，会加入到
/// <see cref="SocketRegionService"/>的队列中 <br/>
/// 2. <see cref="NapCatSocketManager.RemoveSocket(string)"/> 会同时释放socket链接
/// </summary>
file class Note{}

public class NapCatSocketManager
{
    private SocketRegionService region;
    public NapCatSocketManager(SocketRegionService service)
    {
        region = service;
    }
    internal readonly ConcurrentDictionary<string, NapCatHttpSocket> NapCatSockets = [];
    public bool RemoveSocket(string name)
    {
        if(NapCatSockets.TryRemove(name, out var s)) {
            s.Stop();
            return true;
        }
        return false;
    }

    public bool RemoveSocket(NapCatHttpSocket socket)
    {
        foreach (var kv in NapCatSockets.ToArray()) {
            if(kv.Value == socket) {
                return RemoveSocket(kv.Key);
            }
        }
        return false;
    }

    /// <summary>
    /// 创建socket并建立链接和事件下发
    /// </summary>
    /// <param name="name"> socket名称 </param>
    /// <param name="uri"> socket目标地址 例: ws://127.0.0.1:6666 </param>
    /// <param name="password"> socket链接密码 </param>
    /// <param name="exception"> 如果创建失败，如uri无效， 则抛出</param>
    /// <returns> 如果创建成功返回true, 如果创建失败返回false </returns>
    public bool CreateSocket(string name, Uri uri, string password, out Exception? exception)
    {
        exception = null;
        if (NapCatSockets.ContainsKey(name)) {
            exception = new Exception("名称重复。");
            return false;
        }
        try {
            var socket = new NapCatHttpSocket()
            {
                Uri = uri,
                Password = password
            };
            NapCatSockets.TryAdd(name, socket);
            region.Enqueue(new QueueSocketTask(socket)); // 这里可以添加错误回调
        } catch(Exception e) {
            exception = e;
            RemoveSocket(name);
            return false;
        }
        return true;
    }
}
