# [SDK](https://github.com/NanTingPer/NapCatSharp/tree/main/NapCatSharp.Core) 

- 用于辅助调用NapCatQQ提供的API，以及接收Socket下发的事件并转换为具体类型，可用于自定义Bot的开发。

# [MVC](https://github.com/NanTingPer/NapCatSharp/tree/main/NapCatSharp.Mod)

- 用于加载插件，适合直接对接NapCatQQ提供的API，并处理相关事件。

## Mod

> 每一个Mod.dll只能有一个继承于NapCatSharp.Mod.Core.Mod的类型。

Mod会存放在程序根目录下的`Mods`目录中，此目录除了存放Mod外，还有一个`enable.json`表示已经启用的Mod列表。

```json
["TestMod","TestMod2"]
```

`Mods`目录中的每个文件夹都表示一个独立的Mod，单个Mod的全部文件都应当存放到目录中，包括但排除`NapCatSharp.Cor、NapCatSharp.Mod`的全部依赖项。使用自定义的`AssemblyLoadContext`会在`Mods/modname`目录中加载依赖。



## SocketConfig

程序在启动时，会在程序根目录下创建`appconfigs`目录，随后在此目录内创建`sockets.json`，表示已经创建的socket链接，并根据文件内容将`isEnable=true`的对象加入到链接队列。其对象格式为：
```json
{
    "name":"abcd",
    "uri":"ws://127.0.0.1:3001/",
    "password":"625379",
    "isEnable":false
}
```

