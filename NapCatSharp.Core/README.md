## 介绍

基于NapCatQQ框架API的C#封装



## 前置条件

在NapCatQQ网络配置中添加配置信息

- Http服务器: 用于访问各个API
- WebSocketServer: 用于接收下发事件



## 处理消息

处理消息是如此简单
```cs
// 创建socket用于接受napcat的消息事件
var nchs = new NapCatHttpSocket(){ Uri = new Uri("ws://127.0.0.1:3001"), Password = "yVjZkoh5xFxXB_UB" };
// 创建httpServer用于访问napcat的API
var nchttpServer = new NapCatHttpServer(){ Password = "", Uri = new Uri("http://127.0.0.1:3002") };
// 建立socket连接
await nchs.Connection();
// 设置消息事件处理函数
nchs.MetaHeartbeat += Nchs_MetaMessage; // 处理元事件 "心跳"
nchs.MetaLifecycle += Nchs_MetaLifecycle; // 处理元事件 "生命周期"
nchs.MessagePrivate += Nchs_MessagePrivate; // 处理私聊消息事件
//开始接收消息
await nchs.Receive();

async void Nchs_MessagePrivate(NapCatHttpSocket.EventMessageData obj)
{
    // 使用模式匹配 将数据转换为消息类型
    var msgEvent = obj.EventData as PrivateMessageEvent;
    // 回复合并转发消息 "你好"，并使用对方的身份信息
    var abc = await nchttpServer.SendForwardToPrivate(msgEvent!.UserId, [
        Node.Create(new (){
            UserId = msgEvent.UserId,
            Content = [CreateText("你好")],
            Nickname = msgEvent.Sender.Nickname
        }),
    ]);
}

void Nchs_MetaHeartbeat(NapCatHttpSocket.EventMessageData obj)
{
    Console.WriteLine(obj.EventData.ToString());
}

void Nchs_MetaLifecycle(NapCatHttpSocket.EventMessageData obj)
{
    Console.WriteLine(obj.EventData.ToString());
}
```

## 发送合并转发消息
```cs
LongId yourQQ = "123456";
await nchttpServer.SendForwardToPrivate(yourQQ,[
    Node.Create(new (){
        UserId = yourQQ, // 转发中此消息的实际发送人 可伪
        Content = [IOB11MessageModelFlag.CreateText("你好")],
        Nickname = "aaa" // 转发中此消息的显示昵称 可伪
    }),
    ...
]);
```
