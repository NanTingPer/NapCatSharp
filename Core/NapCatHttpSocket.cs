using NapCatSharp.EventPushModels;
using NapCatSharp.EventPushModels.Enums;
using NapCatSharp.Exceptions;
using NapCatSharp.OB11;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
namespace NapCatSharp.Core;

public class NapCatHttpSocket
{
    public record class EventMessageData(NapCatHttpSocket This, EventBaseModel EventData);

    private ClientWebSocket socket;
    public required Uri Uri { get; set; }
    public required string? Password { get; set;}
    /// <summary>
    /// 调用<see cref="Receive(Action{string}, CancellationToken)"/> 开始接收
    /// </summary>
    public event Action<NapCatHttpSocket, WebSocketMessageType, string>? Message = null;
    /// <summary>
    /// 当消息为元事件时候触发
    /// </summary>
    public event Action<EventMessageData>? MetaMessage = null;
    /// <summary>
    /// 当消息为消息事件时触发
    /// </summary>
    public event Action<EventMessageData>? MessageEvent = null;
    /// <summary>
    /// 当消息为请求事件时触发
    /// </summary>
    public event Action<EventMessageData>? RequestEvent = null;
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
                var oEvent = GetEventData<MetaEventType>(doc.RootElement, "meta_event_type");
                if (oEvent != null) {
                    MetaMessage?.Invoke(new (this, oEvent));
                }
            } else if (postType == PostType.message) {
                var oEvent = GetEventData<MessageType>(doc.RootElement, "message_type");
                if (oEvent != null) {
                    MessageEvent?.Invoke(new EventMessageData(this, oEvent));
                }
            } else if (postType == PostType.request) {
                var oEvent = GetEventData<RequestType>(doc.RootElement, "request_type");
                if (oEvent != null) {
                    RequestEvent?.Invoke(new EventMessageData(this, oEvent));
                }
            }
        }
    }

    private static PostType? GetPostType(JsonElement rootElement)
    {
        if (rootElement.TryGetProperty("post_type", out JsonElement el)) {
            var postTypeStr = el.GetString();
            if (postTypeStr == null) return null;
            return EnumExtension<PostType>.GetValue(postTypeStr);
        }
        return null;
    }

    /// <summary>
    /// 根据给定的枚举类型，从<see cref="EnumTypeMap.GetMap{TEnum}"/>中获取对应的类型映射字典。
    /// <br/> 得到类型后，将<paramref name="rootElement"/>反序列化为对应的类型
    /// </summary>
    /// <returns></returns>
    private static EventBaseModel? GetEventData<TEnum>(JsonElement rootElement, string enumPropName)
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
    private static JsonElement StringKeyToObject(JsonElement rootElement)
    {
        if (rootElement.ValueKind == JsonValueKind.String) {
            var reUtf8JsonReader = new Utf8JsonReader(Encoding.UTF8.GetBytes(rootElement.ToString()));
            rootElement = JsonElement.ParseValue(ref reUtf8JsonReader);
        }
        return rootElement;
    }
}