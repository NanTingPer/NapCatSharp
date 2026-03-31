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

`Mods`目录中的每个文件夹都表示一个独立的Mod，单个Mod的全部文件都应当存放到目录中，包括但排除`NapCatSharp.Cor、NapCatSharp.Mod`的全部依赖项。

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

## ModConfig

在Mod类型被实例化之前，会先行创建ModConfig的实例，一个模组dll能够拥有多个ModConfig但只能拥有一个Mod。
注意，无论如何，都不要在模组程序集中持有ModConfig的强引用，而是要使用时重新获取，否则可能无法正确卸载，导致内存残留 / 泄露。

### 创建

要创建配置，只需要使类型继承ModConfig，最终序列化时，使用`System.Text.Json.Serialization`，因此，当你不希望某个属性被序列化时，可以使用`[JsonIgnore]`特性。
```cs
public class TestModConfig : ModConfig
{
    public string 测试配置项 { get; set; } = "你好测试配置项目";
    public List<string> 测试配置项集合 { get; set; } = ["哈哈", "哈韩"];
}
```

### 结果

配置文件被保存在本地，`AppPath/configs/mods`中，此文件夹中的每个文件夹都表示一个模组配置文件夹。
如上图的Config序列化后的结果为

```json
{
  "测试配置项": "你好测试配置项目",
  "测试配置项集合": [
    "哈哈",
    "哈韩"
  ]
}
```

### 获取

在Mod实例中，从父类继承了`GetConfig`方法，若要获取配置对象，调用即可，其要求传入配置对象的泛型。
```cs
var config =  GetConfig<TestModConfig>();
```

