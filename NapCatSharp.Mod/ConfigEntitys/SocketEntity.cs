using NapCatSharp.Core;
using System.Text.Json.Serialization;

namespace NapCatSharp.Mod.ConfigEntitys;

public class SocketEntity
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("uri")]
    public string Uri { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    [JsonPropertyName("isEnable")]
    public bool IsEnable { get; set; } = false;

    /// <summary>
    /// 当前连接状态（运行时，不持久化到 JSON）
    /// </summary>
    [JsonIgnore]
    public ConnectionStatus Status { get; set; } = ConnectionStatus.Disconnected;

    /// <summary>
    /// 当前重试次数（运行时，不持久化到 JSON）
    /// </summary>
    [JsonIgnore]
    public int RetryCount { get; set; } = 0;

    public static SocketEntity Create(string name, NapCatHttpSocket socket)
    {
        return new SocketEntity()
        {
             IsEnable = true,
             Name = name,
             Password = socket.Password ?? "",
             Uri = socket.Uri.ToString()
        };
    }
}
