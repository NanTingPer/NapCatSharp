using System.Net.WebSockets;
using System.Text;
namespace NapCatSharp.Core;

public class NapCatHttpSocket
{
    private ClientWebSocket socket;
    public required Uri Uri { get; set; }
    public required string? Password { get; set;}
    /// <summary>
    /// 调用<see cref="Receive(Action{string}, CancellationToken)"/> 开始接收
    /// </summary>
    public event Action<NapCatHttpSocket, WebSocketMessageType, string>? Message = null;
    public NapCatHttpSocket()
    {
        socket = new ClientWebSocket();
    }

    /// <summary>
    /// 使用给定的Uri进行连接
    /// </summary>
    /// <param name="cancellationToken"> 停止令牌，传递给 socket.ConnectAsync </param>
    /// <returns></returns>
    public Task Connection(CancellationToken cancellationToken = default)
    {
        if(socket.State == WebSocketState.Open) {
            return Task.CompletedTask;
        }
        if(Password != null) {
            socket.Options.SetRequestHeader("Authorization", $"Bearer {Password}");
        }
        return socket.ConnectAsync(Uri, cancellationToken);
    }

    /// <summary>
    /// 接收消息
    /// </summary>
    /// <exception cref="SocketConnectionException"> socket链接失败 </exception>
    public async Task Receive(CancellationToken token = default)
    {
        if(socket.State == WebSocketState.None) {
            try {
                await Connection(token);
            } catch(Exception e) {
                throw new SocketConnectionException(e.Message);
            }
        }
        ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[256]);
        while(token.IsCancellationRequested == false) {
            using MemoryStream allMemoryStream = new MemoryStream();
            bool receiveIsEnd = false;
            WebSocketMessageType type;
            do {
                var receive = await socket.ReceiveAsync(buffer, token);
                type = receive.MessageType;
                if (receive.MessageType == WebSocketMessageType.Text) {
                    allMemoryStream.Write(buffer.Array!, 0, receive.Count);
                    receiveIsEnd = receive.EndOfMessage;
                } else if(receive.MessageType == WebSocketMessageType.Close) {
                    await socket.CloseAsync(WebSocketCloseStatus.Empty, null, token);
                }
            } while (receiveIsEnd == false && token.IsCancellationRequested == false);
            var message = Encoding.UTF8.GetString(allMemoryStream.ToArray());
            Message?.Invoke(this, type, message);
        }
    }
}

/// <summary>
/// Socket未被打开错误
/// </summary>
/// <param name="message"></param>
public class SocketNotOpenException(string message) : Exception
{
    public override string Message => message;
}

/// <summary>
/// Socket连接失败错误
/// </summary>
/// <param name="message"></param>
public class SocketConnectionException(string message) : Exception
{
    public override string Message => message;
}