using NapCatSharp.Core;
using NapCatSharp.Mod.ConfigEntitys;
using NapCatSharp.Mod.Services;
using System.Collections.Concurrent;
using System.Text.Json;

namespace NapCatSharp.Mod.Core;

/// <summary>
/// 1. <see cref="NapCatSocketManager"/> 用于在Web / 控制器中创建Socket <br/>
/// - <see cref="NapCatSocketManager.Create(string, Uri, string, out Exception?)"/> 负责创建实例并启动独立后台连接任务 <br/>
/// 2. <see cref="NapCatSocketManager.Disable(string)"/> 会同时通过 CancellationTokenSource 取消 socket 的重试/连接/接收
/// </summary>
public class NapCatSocketManager
{
    public static readonly string socketListConfigPath = Path.Combine(AppContext.BaseDirectory, "appconfigs", "sockets.json");
    public static readonly string configDir = Path.Combine(AppContext.BaseDirectory, "appconfigs");
    private static readonly Lock configtouringLock = new Lock();
    public static List<SocketEntity> Configs
    {
        get
        {
            string? configValue = null;
            lock (configtouringLock) {
                try {
                    configValue = File.ReadAllText(socketListConfigPath);
                } catch { }
            }
            return JsonSerializer.Deserialize<List<SocketEntity>>(configValue ?? "[]") ?? [];
        }
        set
        {
            var jsonv = JsonSerializer.Serialize(value.ToHashSet());
            lock (configtouringLock) {
                try {
                    File.WriteAllText(socketListConfigPath, jsonv ?? "[]");
                } catch { }
            }
        }
    }

    private readonly SocketRegionService region;
    private readonly SystemLogger logger;

    /// <summary>
    /// key: socket 名称, value: CancellationTokenSource
    /// </summary>
    private readonly ConcurrentDictionary<string, CancellationTokenSource> socketCtsMap = [];

    /// <summary>
    /// key: socket 名称, value: 运行时状态信息 (Status, RetryCount)
    /// </summary>
    private readonly ConcurrentDictionary<string, (ConnectionStatus Status, int RetryCount)> runtimeStatus = [];

    public NapCatSocketManager(SocketRegionService service, SystemLogger log)
    {
        region = service;
        logger = log;
        if (!File.Exists(socketListConfigPath)) {
            logger.Info("Socket配置文件不存在，创建。");
            if (!Directory.Exists(configDir)) {
                Directory.CreateDirectory(configDir);
            }
            var str = File.CreateText(socketListConfigPath);
            str.WriteLine("[]");
            str.Dispose();
        }
        foreach (var item in Configs.Where(f => f.IsEnable == true)) {
            try {
                logger.Info($"启用Socket: {item.Name} {item.Uri}");
                Enable(item);
            } catch { }
        }
    }

    /// <summary>
    /// key是socket的名称
    /// </summary>
    internal readonly ConcurrentDictionary<string, NapCatHttpSocket> NapCatSockets = [];

    /// <summary>
    /// 关闭socket（取消后台任务，停止连接）
    /// </summary>
    public void Disable(string name)
    {
        // 取消后台任务
        if (socketCtsMap.TryRemove(name, out var cts)) {
            cts.Cancel();
            cts.Dispose();
        }
        // 移除 socket 实例
        if (NapCatSockets.TryRemove(name, out var s)) {
            s.Stop();
        }
        // 清理运行时状态
        runtimeStatus.TryRemove(name, out _);

        var c = Configs;
        var entity = c.FirstOrDefault(f => f.Name == name);
        if (entity != null) {
            logger.Info($"禁用Socket: {entity.Name} {entity.Uri}");
            entity.IsEnable = false;
            entity.Status = ConnectionStatus.Disconnected;
            entity.RetryCount = 0;
            Configs = c;
        }
    }

    /// <summary>
    /// 关闭socket
    /// </summary>
    public void Close(NapCatHttpSocket socket)
    {
        foreach (var kv in NapCatSockets.ToArray()) {
            if (kv.Value == socket) {
                Disable(kv.Key);
            }
        }
    }

    public void Enable(SocketEntity entity)
    {
        var socket = new NapCatHttpSocket()
        {
            Uri = new Uri(entity.Uri),
            Password = entity.Password
        };
        NapCatSockets.TryAdd(entity.Name, socket);

        // 创建可取消的 CancellationTokenSource
        var cts = new CancellationTokenSource();
        socketCtsMap.TryAdd(entity.Name, cts);

        // 初始化运行时状态
        runtimeStatus[entity.Name] = (ConnectionStatus.Connecting, 0);

        logger.Info($"启用Socket: {entity.Name} {entity.Uri}");
        region.StartSocket(entity.Name, socket, cts, OnStatusChanged);

        entity.IsEnable = true;
        entity.Status = ConnectionStatus.Connecting;
        entity.RetryCount = 0;
        Configs = Configs;
    }

    public void Enable(string name, string uri, string? password)
    {
        var entity = new SocketEntity()
        {
            Name = name,
            Uri = uri,
            Password = password ?? ""
        };
        Enable(entity);
    }

    public void Enable(string name)
    {
        var c = Configs;
        var entity = c.FirstOrDefault(f => f.Name == name);
        if (entity != null) {
            entity.IsEnable = true;
            Configs = c; // 先持久化 IsEnable=true 到文件，再启动连接
            Enable(entity);
        }
    }

    /// <summary>
    /// 关闭socket并删除配置
    /// </summary>
    public void Delete(string name)
    {
        if (NapCatSockets.ContainsKey(name)) {
            Disable(name);
        }
        DeleteConfig(name);
    }

    /// <summary>
    /// 创建socket并建立链接和事件下发
    /// </summary>
    /// <param name="name"> socket名称 </param>
    /// <param name="uri"> socket目标地址 例: ws://127.0.0.1:6666 </param>
    /// <param name="password"> socket链接密码 </param>
    /// <param name="exception"> 如果创建失败，如uri无效， 则抛出</param>
    /// <returns> 如果创建成功返回true, 如果创建失败返回false </returns>
    public bool Create(string name, Uri uri, string password, out Exception exception)
    {
        exception = null!;
        if (Configs.Any(f => f.Name == name)) {
            exception = new Exception("名称重复。");
            return false;
        }
        try {
            var entity = new SocketEntity()
            {
                Name = name,
                Uri = uri.ToString(),
                IsEnable = true,
                Password = password ?? ""
            };
            Enable(entity);
            AddConfig(entity);
        } catch (Exception e) {
            exception = e;
            Disable(name);
            return false;
        }
        return true;
    }

    /// <summary>
    /// 将内容添加到配置
    /// </summary>
    private void AddConfig(SocketEntity entity)
    {
        var c = Configs;
        c.Add(entity);
        Configs = c;
    }

    /// <summary>
    /// 删除给定配置
    /// </summary>
    private void DeleteConfig(string name)
    {
        var c = Configs;
        int count = c.RemoveAll(f => f.Name == name);
        if (count == 0) return;
        Configs = c;
        logger.Info($"删除Socket: {name}");
    }

    /// <summary>
    /// 状态变更回调：更新运行时状态和 SocketEntity
    /// </summary>
    private void OnStatusChanged(string name, ConnectionStatus status, int retryCount)
    {
        runtimeStatus[name] = (status, retryCount);

        // 同步更新到 Configs 中的 SocketEntity
        var c = Configs;
        var entity = c.FirstOrDefault(f => f.Name == name);
        if (entity != null) {
            entity.Status = status;
            entity.RetryCount = retryCount;
            Configs = c;
        }
    }

    /// <summary>
    /// 全部socket集合 包括未启用的
    /// </summary>
    public List<SocketEntity> Sockets
    {
        get
        {
            var configs = Configs;
            // 同步运行时状态到返回结果
            foreach (var entity in configs) {
                if (runtimeStatus.TryGetValue(entity.Name, out var rt)) {
                    entity.Status = rt.Status;
                    entity.RetryCount = rt.RetryCount;
                } else {
                    // 没有运行时状态 = 已禁用/已停止
                    entity.Status = entity.IsEnable ? ConnectionStatus.Connecting : ConnectionStatus.Disconnected;
                    entity.RetryCount = 0;
                }
            }
            return configs;
        }
    }
}