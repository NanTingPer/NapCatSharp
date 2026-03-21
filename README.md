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
nchs.MessageEvent += GetMsgInfo;
//开始接收消息
await nchs.Receive();

async void GetMsgInfo(NapCatHttpSocket.EventMessageData data)
{
    // 使用模式匹配可以轻松知道消息类型
    if(data.EventData is GroupMessageEvent groupEvent) {
        var msgInfo = await nchttpServer.GetMsgInfo(groupEvent.MessageId);
    }

    if(data.EventData is PrivateMessageEvent privateEvent) {
        // 使用模式匹配 轻松获取全部的 `Text` 类型的消息
        IEnumerable<Text> allText = privateEvent.Message.Where(ob11Msg => ob11Msg is Text)
                .Select(text => (text as Text)!);

        var nihaoText = allText.FirstOrDefault(text => "你好".Equals(text));
        if (nihaoText != null) {
            var yourQQId = 123456L;
            await nchttpServer.SendPrivateTextMsg($"收到来自{privateEvent.UserId}的消息",yourQQId);
        }
    }
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

## 项目结构

```
NapCatSharp/
├── Core/                           # 核心功能
│   ├── NapCatHttpServer.cs        # HTTP服务器，用于访问NapCat API
│   ├── NapCatHttpServer-MessageInterface.cs
│   └── NapCatHttpSocket.cs        # WebSocket客户端，用于接收NapCat消息事件
│
├── EventPushModels/               # NapCat推送的消息数据模型
│   ├── MessageEvents/             # 消息事件模型
│   │   ├── GroupMessageEvent.cs   # 群消息事件
│   │   ├── GroupSender.cs         # 群发送者信息
│   │   ├── PrivateMessageEvent.cs # 私聊消息事件
│   │   ├── PrivateSender.cs       # 私聊发送者信息
│   │   ├── RoleEnum.cs            # 角色枚举
│   │   └── SexEnum.cs             # 性别枚举
│   │
│   ├── MetaEvents/                # 元事件模型
│   │   ├── Heartbeat.cs           # 心跳事件
│   │   └── Lifecycle.cs           # 生命周期事件
│   │
│   ├── EventBaseModel.cs          # 事件基类
│   ├── EventBaseModelG.cs         # 通用事件基类
│   ├── PostType.cs                # 事件类型枚举
│   ├── MetaEventType.cs           # 元事件类型
│   ├── EnumExtension.cs           # 枚举扩展
│   └── EnumTypeMap.cs             # 枚举类型映射
│
├── OB11/                          # OB11协议相关
│   ├── OB11MessageModels/         # OB11消息类型
│   │   ├── At.cs                  # @消息
│   │   ├── Face.cs                # 表情
│   │   ├── Forward.cs             # 转发消息
│   │   ├── Image.cs               # 图片
│   │   ├── MFace.cs               # 魔法表情
│   │   ├── Node.cs                # 转发节点
│   │   ├── Poke.cs                # 戳一戳
│   │   ├── Reply.cs               # 回复
│   │   └── Text.cs                # 文本
│   │
│   ├── OB11MessageModelBase.cs    # OB11消息基类
│   ├── OB11MessageType.cs         # OB11消息类型枚举
│   ├── MessageType.cs             # 消息类型
│   └── IOB11MessageModelFlag.cs   # OB11消息标志接口
│
├── RequestModels/                 # 请求NapCat接口的数据模型
│   ├── MessageInterfaces/         # NapCat API消息接口
│   │   ├── DeleteMsg.cs           # 删除消息
│   │   ├── ForwardFriendSingleMsg.cs # 转发好友单条消息
│   │   ├── ForwardGroupSingleMsg.cs  # 转发群单条消息
│   │   ├── GetMsg.cs              # 获取消息
│   │   ├── GetMsgResponse.cs      # 获取消息响应
│   │   ├── MarkAllAsRead.cs       # 全部标记已读
│   │   ├── MarkGroupMsgAsRead.cs  # 群消息标记已读
│   │   ├── MarkPrivateMsgAsRead.cs # 私聊消息标记已读
│   │   ├── SendMsg.cs             # 发送消息
│   │   └── SendPrivateMsg.cs      # 发送私聊消息
│   │
│   ├── RequestModelBase.cs        # 请求模型基类
│   └── RequestModelResponseBase.cs # 请求响应基类
│
├── JsonConverter/                 # JSON转换器
│   ├── EnumJsonConverter.cs       # 枚举转换器
│   ├── LongIdConver.cs            # Long ID转换器
│   ├── StringIdConverter.cs      # String ID转换器
│   ├── OB11MessageModelFlagConver.cs # OB11消息标志转换器
│   ├── OB11MessageTypeConver.cs   # OB11消息类型转换器
│   └── PostTypeConverter.cs       # 事件类型转换器
│
├── Exceptions/                    # 异常处理
│   ├── SocketConnectionException.cs # Socket连接异常
│   └── SocketNotOpenException.cs   # Socket未打开异常
│
├── Utils/                         # 工具类
│   └── ListExtension.cs           # List扩展方法
│
├── LongId.cs                      # Long ID类型（隐式类型转换）
├── StringId.cs                    # String ID类型（隐式类型转换）
├── NapCatSharp.csproj             # 项目文件
├── NapCatSharp.sln                # 解决方案文件
└── README.md                      # 项目说明文档
```