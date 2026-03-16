```
|RequestModels -> 用于请求NapCat接口的数据模型
----|
    | OB11MessageModelBase -> OB11的消息类型基类
    | OB11MessageType -> OB11的消息类型枚举
    | LongId / StringId -> long 到 string 的隐士类型转换
    |
    | MessageInterfaces -> NapCat API 中的消息接口
    | OB11MessageModels -> OB11的消息类型
    | 
|EventPushModels -> NapCat推送的消息数据模型
```

## 发送消息
```cs
var nchs = new NapCatHttpServer()
{ 
    Uri = new Uri("http://127.0.0.1:3002"),
    Password = ""
};

var targetQQ = "123456";
await nchs.SendPrivateTextMsg("你好", targetQQ); // 将 "你好" 发送到 目标QQ
```

## 接收消息
```cs
var nchs = new NapCatHttpSocket(){ Uri = new Uri("ws://127.0.0.1:3001"), Password = "yVjZ" };
await nchs.Connection();
nchs.Message += Nchs_Message;
await nchs.Receive();

void Nchs_Message(NapCatHttpSocket arg1, System.Net.WebSockets.WebSocketMessageType arg2, string arg3)
{
    Console.WriteLine(arg3);
}
```