namespace NapCatSharp.Mod.ConfigEntitys;

/// <summary>
/// Socket 连接状态
/// </summary>
public enum ConnectionStatus
{
    /// <summary>
    /// 已断开
    /// </summary>
    Disconnected = 0,

    /// <summary>
    /// 连接中（含重试中）
    /// </summary>
    Connecting = 1,

    /// <summary>
    /// 已连接
    /// </summary>
    Connected = 2
}