using NapCatSharp.EventPushModels;
using NapCatSharp.EventPushModels.MessageEvents;
using NapCatSharp.Exceptions;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
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
    public event Action<NapCatHttpSocket, EventBaseModel>? MetaEventMessage = null;
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

            var doc = JsonSerializer.SerializeToDocument(message);
            doc = JsonSerializer.SerializeToDocument(StringKeyToObject(doc.RootElement));
            var postType = GetPostType(doc.RootElement);
            // 解析数据
            if (postType == null) continue;
            if (postType == PostType.meta_event) {
                var oEvent = GetMeatEvent(doc.RootElement);
                if (oEvent != null) {
                    MetaEventMessage?.Invoke(this, oEvent);
                }
            } else if (postType == PostType.message) {
                var oEvent = GetMessageEvent(doc.RootElement);
                if (oEvent != null) {
                    MetaEventMessage?.Invoke(this, oEvent);
                }
            }
        }
    }

    public static PostType? GetPostType(JsonElement rootElement)
    {
        //rootElement = StringKeyToObject(rootElement);
        if (rootElement.TryGetProperty("post_type", out JsonElement el)) {
            var postTypeStr = el.GetString();
            if (postTypeStr == null) return null;
            return EnumExtension<PostType>.GetValue(postTypeStr);
        }
        return null;
    }

    public static EventBaseModel? GetMeatEvent(JsonElement rootElement)
    {
        if (!rootElement.TryGetProperty("meta_event_type", out JsonElement eventType)) {
            return null;
        }

        var eventTypeStr = eventType.GetString();
        if (eventTypeStr == null) return null;

        var type = EnumExtension<MetaEventType>.GetValue(eventTypeStr);
        if(type == null) return null;

        if (EnumTypeMap.MetaEventTypeMap.TryGetValue(type.Value, out var otype)) {
            return (EventBaseModel)rootElement.Deserialize(otype)!;
        }
        return null;
    }

    public static EventBaseModel? GetMessageEvent(JsonElement rootElement)
    {
        if (!rootElement.TryGetProperty("message_type", out JsonElement eventType)) {
            return null;
        }

        var eventTypeStr = eventType.GetString();
        if (eventTypeStr == null) return null;

        var type = EnumExtension<MessageType>.GetValue(eventTypeStr);
        if (type == null) return null;

        if (EnumTypeMap.MessageEventTypeMap.TryGetValue(type.Value, out var otype)) {
            return (EventBaseModel)rootElement.Deserialize(otype)!;
        }
        return null;
    }

    public static EventBaseModel? GetEventData<TEnum>(JsonElement rootElement, string enumPropName)
        where TEnum : struct, Enum
    {
        if (!rootElement.TryGetProperty(enumPropName, out JsonElement eventType)) {
            return null;
        }

        var eventTypeStr = eventType.GetString();
        if (eventTypeStr == null) return null;

        var type = EnumExtension<TEnum>.GetValue(eventTypeStr);
        if (type == null) return null;

        if (EnumTypeMap.GetMap<TEnum>()!.TryGetValue(type.Value, out var otype)) {
            return (EventBaseModel)rootElement.Deserialize(otype)!;
        }
        return null;
    }


    /// <summary>
    /// 将post的string内容转为object
    /// </summary>
    /// <returns></returns>
    public static JsonElement StringKeyToObject(JsonElement rootElement)
    {
        if (rootElement.ValueKind == JsonValueKind.String) {
            var reUtf8JsonReader = new Utf8JsonReader(Encoding.UTF8.GetBytes(rootElement.ToString()));
            rootElement = JsonElement.ParseValue(ref reUtf8JsonReader);
        }
        return rootElement;
    }
}