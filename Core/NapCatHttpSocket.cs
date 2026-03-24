using NapCatSharp.EventPushModels;
using NapCatSharp.EventPushModels.Enums;
using NapCatSharp.Exceptions;
using NapCatSharp.OB11;
using NapCatSharp.Utils;

using System.Collections.Frozen;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Reflection;
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

    private delegate EventBaseModel? GetDataDelegate(JsonElement rootElement, string propName, out Enum? enumValue);
    #region static ctor
    private readonly static List<Type> enumTypes = [typeof(MessageType), typeof(MetaType), typeof(RequestType)];
    private readonly static FrozenDictionary<Type, GetDataDelegate> subTypeGetData;
    private readonly static FrozenDictionary<Type, string> typeToPropName = new Dictionary<Type, string>()
    {
        { typeof(MessageType), "message_type" },
        { typeof(RequestType), "request_type" },
        { typeof(MetaType), "meta_event_type" },
    }.ToFrozenDictionary();

    static NapCatHttpSocket()
    {
        subTypeGetData = GetEventDataFuncs();
    }
    #endregion

    /// <summary>
    /// key => 枚举类型 <br/>
    ///     - <see cref="MessageType"/> <br/>
    ///     - <see cref="RequestType"/> <br/>
    ///     - <see cref="MetaType"/> <br/>
    /// value => 子类型对应的事件
    /// </summary>
    private readonly Dictionary<Type, Dictionary<Enum, List<Action<EventMessageData>>>> subTypeEvent;
    
    #region MetaEvent
    public event Action<EventMessageData> MetaLifecycle
    {
        remove => metaLifecycle -= value;
        add => metaLifecycle += value;
    }
    private List<Action<EventMessageData>> metaLifecycle = [];

    public event Action<EventMessageData> MetaHeartbeat
    {
        remove => metaHeartbeat -= value;
        add => metaHeartbeat += value;
    }
    private List<Action<EventMessageData>> metaHeartbeat = [];
    #endregion

    #region MessageEvent
    public event Action<EventMessageData>? MessagePrivate
    {
        remove => messagePrivate -= value;
        add => messagePrivate += value;
    }
    private List<Action<EventMessageData>> messagePrivate = [];

    public event Action<EventMessageData>? MessageGroup
    {
        remove => messageGroup -= value;
        add => messageGroup += value;
    }
    private List<Action<EventMessageData>> messageGroup = [];
    #endregion

    #region RequestEvent
    public event Action<EventMessageData>? RequestFriend
    {
        remove => requestFriend -= value;
        add => requestFriend += value;
    }
    private List<Action<EventMessageData>> requestFriend = [];

    public event Action<EventMessageData>? RequestGroup
    {
        remove => requestGroup -= value;
        add => requestGroup += value;
    }
    private List<Action<EventMessageData>> requestGroup = [];
    #endregion

    public NapCatHttpSocket()
    {
        socket = new ClientWebSocket();

        #region subTypeEvent
        subTypeEvent = new Dictionary<Type, Dictionary<Enum, List<Action<EventMessageData>>>>()
        {
            { 
                typeof(MetaType), new()
                {
                    { MetaType.lifecycle, metaLifecycle },
                    { MetaType.heartbeat, metaHeartbeat }
                }
            },
            {
                typeof(RequestType), new()
                {
                    { RequestType.friend, requestFriend },
                    { RequestType.group, requestGroup }
                }
            },
            {
                typeof(MessageType), new()
                {
                    { MessageType.@private, messagePrivate },
                    { MessageType.group, messageGroup }
                }
            }
        };
        #endregion
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
            // 分发事件
            EventDispatch(doc, postType);
        }
    }

    private void EventDispatch(JsonDocument jsonDoc, PostType? postType)
    {
        // 使用字典存储 event Action
        // 构建PostType类型对应访问GetEventData<T>的表达式树
        //  - 字典为D<PostType, Func<JsonElement, string, Type, EventBaseModel>>
        //  - PostType => 消息类型
        //  - JsonElement => jsonDoc.RootElement
        //  - string => jsonPropName 子枚举类型在json中的属性key
        //  - Type => 子枚举类型
        if (postType == null) return;
        // 获取子类型
        var subType = postType.Value.SubType;
        // 从subTypeGetData 获取 GetEventData<Type>
        if (!subTypeGetData.TryGetValue(subType, out var getEventData)) {
            return;
        }
        var eventBase = getEventData.Invoke(jsonDoc.RootElement, typeToPropName[subType], out Enum? enumValue);
        if(eventBase == null) return;
        if (!subTypeEvent.TryGetValue(subType, out var subevent)) {
            return;
        }
        subevent[enumValue!]?.ForEach(ac => 
            ac.Invoke(new EventMessageData(this, eventBase)));
    }

    /// <summary>
    /// 获取每个消息子类型的Func
    /// </summary>
    private static FrozenDictionary<Type, GetDataDelegate> GetEventDataFuncs()
    {
        var genMethodInfo = typeof(NapCatHttpSocket).GetMethod(nameof(GetEventData), BindingFlags.NonPublic | BindingFlags.Static);
        Dictionary<Type, GetDataDelegate> cache = [];
        foreach (var postType in enumTypes) {
            var markMethodInfo = genMethodInfo!.MakeGenericMethod(postType);
            var reParameter = Expression.Parameter(typeof(JsonElement));
            var nameParameter = Expression.Parameter(typeof(string));
            var enumValueParameter = Expression.Parameter(typeof(Enum).MakeByRefType());
            var callExpression = Expression.Call(markMethodInfo, reParameter, nameParameter, enumValueParameter);
            var getEventData = Expression.Lambda<GetDataDelegate>(callExpression, reParameter, nameParameter, enumValueParameter).Compile();
            cache[postType] = getEventData!;
        }
        return cache.ToFrozenDictionary();
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
    private static EventBaseModel? GetEventData<TEnum>(JsonElement rootElement, string enumPropName, out Enum? enumValue)
        where TEnum : struct, Enum
    {
        enumValue = null;
        if (!rootElement.TryGetProperty(enumPropName, out JsonElement eventType)) {
            return null;
        }

        var eventTypeStr = eventType.GetString();
        if (eventTypeStr == null) return null;

        var type = EnumExtension<TEnum>.GetValue(eventTypeStr);
        enumValue = type;
        if (type == null) return null;

        // 获取对应子类型的实体类型
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