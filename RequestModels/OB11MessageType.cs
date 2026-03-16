using NapCatSharp.RequestModels.JsonConverter;
using System.Collections.Frozen;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace NapCatSharp.RequestModels;

public enum OB11MessageTypeEnum
{
    #region 枚举
    /// <summary> 纯文本 </summary>
    text,
    /// <summary> 表情 </summary>
    face,
    /// <summary> 商城表情 </summary>
    mface,
    /// <summary> at </summary> 
    at,
    /// <summary> 回复 </summary>
    reply,
    /// <summary> 图片 </summary>
    image,
    /// <summary> 语音 </summary>
    record,
    /// <summary> 视频 </summary>
    video,
    /// <summary> 文件 </summary>
    file,
    /// <summary> 音乐 </summary>
    music,
    /// <summary> 戳一戳 </summary> 
    poke,
    /// <summary> 骰子 </summary>
    dice,
    /// <summary> 猜拳 </summary>
    rps,
    /// <summary> 联系人 </summary>
    contact,
    /// <summary> 位置 </summary>
    location,
    /// <summary> JSON </summary>
    json,
    /// <summary> XML </summary>
    xml,
    /// <summary> Markdown </summary>
    markdown,
    /// <summary> 小程序 </summary>
    miniapp,
    /// <summary> 合并转发节点 </summary>
    node,
    /// <summary> 合并转发 </summary>
    forward,
    /// <summary> 在线文件 </summary>
    onlinefile,
    /// <summary> QQ闪传 </summary>
    flashtransfer,
    #endregion
}

/// <summary>
/// OB11 消息类型
/// </summary>
[JsonConverter(typeof(OB11MessageTypeConver))]
public readonly struct OB11MessageType
{
    #region ModuleInitializer + 初始化messageTypeMap
    static OB11MessageType()
    {
        #region 类型名称到此类型的映射
        typeNameMap = new Dictionary<string, OB11MessageType>(){
            { "text", Text! },
            { "face", Face! },
            { "mface", Mface! },
            { "at", At! },
            { "reply", Reply! },
            { "image", Image! },
            { "record", Record! },
            { "video", Video! },
            { "file", File! },
            { "music", Music! },
            { "poke", Poke! },
            { "dice", Dice! },
            { "rps", Rps! },
            { "contact", Contact! },
            { "location", Location! },
            { "json", Json! },
            { "xml", Xml! },
            { "markdown", Markdown! },
            { "miniapp", Miniapp! },
            { "node", Node! },
            { "forward", Forward! },
            { "onlinefile", Onlinefile! },
            { "flashtransfer", Flashtransfer! },
        }.ToFrozenDictionary();
        #endregion

        #region 枚举类型到此类型的映射
        enumNameMap = new Dictionary<OB11MessageTypeEnum, OB11MessageType>()
        {
            { OB11MessageTypeEnum.text, Text! },
            { OB11MessageTypeEnum.face, Face! },
            { OB11MessageTypeEnum.mface, Mface! },
            { OB11MessageTypeEnum.at, At! },
            { OB11MessageTypeEnum.reply, Reply! },
            { OB11MessageTypeEnum.image, Image! },
            { OB11MessageTypeEnum.record, Record! },
            { OB11MessageTypeEnum.video, Video! },
            { OB11MessageTypeEnum.file, File! },
            { OB11MessageTypeEnum.music, Music! },
            { OB11MessageTypeEnum.poke, Poke! },
            { OB11MessageTypeEnum.dice, Dice! },
            { OB11MessageTypeEnum.rps, Rps! },
            { OB11MessageTypeEnum.contact, Contact! },
            { OB11MessageTypeEnum.location, Location! },
            { OB11MessageTypeEnum.json, Json! },
            { OB11MessageTypeEnum.xml, Xml! },
            { OB11MessageTypeEnum.markdown, Markdown! },
            { OB11MessageTypeEnum.miniapp, Miniapp! },
            { OB11MessageTypeEnum.node, Node! },
            { OB11MessageTypeEnum.forward, Forward! },
            { OB11MessageTypeEnum.onlinefile, Onlinefile! },
            { OB11MessageTypeEnum.flashtransfer, Flashtransfer! },
        }.ToFrozenDictionary();
        #endregion

        messageTypeMap = typeof(OB11MessageType).Assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IOB11MessageModelFlag)) && t.IsAbstract == false && t.IsInterface == false)
            .Select(t => {
                OB11MessageType flagType = ((IOB11MessageModelFlag)Activator.CreateInstance(t)!)!.GetMessageType();
                return KeyValuePair.Create(flagType, t);
            })
            .ToFrozenDictionary()
            ;
    }

#pragma warning disable CA2255 // 不应在库中使用 “ModuleInitializer” 属性
    [ModuleInitializer]
#pragma warning restore CA2255 // 不应在库中使用 “ModuleInitializer” 属性
    internal static void Init(){}
    #endregion

    public string TypeName { get; init; }

    /// <summary>
    /// <see cref="OB11MessageType"/> -> <see cref="OB11MessageModelBase{TModel}"/>
    /// </summary>
    public readonly static FrozenDictionary<OB11MessageType, Type> messageTypeMap;

    #region 类型名称到此类型的映射
    public readonly static FrozenDictionary<string, OB11MessageType> typeNameMap;
    #endregion

    #region 枚举类型到此类型的映射
    public static readonly FrozenDictionary<OB11MessageTypeEnum, OB11MessageType> enumNameMap;
    #endregion

    private OB11MessageType(string typeName)
    {
        TypeName = typeName;
    }

    #region 预定义类型
    /// <summary> 纯文本 </summary>
    public readonly static OB11MessageType Text = new OB11MessageType("text");

    /// <summary> 表情 </summary>
    public readonly static OB11MessageType Face = new OB11MessageType("face");

    /// <summary> 商城表情 </summary>
    public readonly static OB11MessageType Mface = new OB11MessageType("mface");

    /// <summary> at </summary>
    public readonly static OB11MessageType At = new OB11MessageType("at");

    /// <summary> 回复 </summary>
    public readonly static OB11MessageType Reply = new OB11MessageType("reply");

    /// <summary> 图片 </summary>
    public readonly static OB11MessageType Image = new OB11MessageType("image");

    /// <summary> 语音 </summary>
    public readonly static OB11MessageType Record = new OB11MessageType("record");

    /// <summary> 视频 </summary>
    public readonly static OB11MessageType Video = new OB11MessageType("video");

    /// <summary> 文件 </summary>
    public readonly static OB11MessageType File = new OB11MessageType("file");

    /// <summary> 音乐 </summary>
    public readonly static OB11MessageType Music = new OB11MessageType("music");

    /// <summary> 戳一戳 </summary>
    public readonly static OB11MessageType Poke = new OB11MessageType("poke");

    /// <summary> 骰子 </summary>
    public readonly static OB11MessageType Dice = new OB11MessageType("dice");

    /// <summary> 猜拳 </summary>
    public readonly static OB11MessageType Rps = new OB11MessageType("rps");

    /// <summary> 联系人 </summary>
    public readonly static OB11MessageType Contact = new OB11MessageType("contact");

    /// <summary> 位置 </summary>
    public readonly static OB11MessageType Location = new OB11MessageType("location");

    /// <summary> JSON </summary>
    public readonly static OB11MessageType Json = new OB11MessageType("json");

    /// <summary> XML </summary>
    public readonly static OB11MessageType Xml = new OB11MessageType("xml");

    /// <summary> Markdown </summary>
    public readonly static OB11MessageType Markdown = new OB11MessageType("markdown");

    /// <summary> 小程序 </summary>
    public readonly static OB11MessageType Miniapp = new OB11MessageType("miniapp");

    /// <summary> 合并转发节点 </summary>
    public readonly static OB11MessageType Node = new OB11MessageType("node");

    /// <summary> 合并转发 </summary>
    public readonly static OB11MessageType Forward = new OB11MessageType("forward");

    /// <summary> 在线文件 </summary>
    public readonly static OB11MessageType Onlinefile = new OB11MessageType("onlinefile");

    /// <summary> QQ闪传 </summary>
    public readonly static OB11MessageType Flashtransfer = new OB11MessageType("flashtransfer");
    #endregion

    public static implicit operator string(OB11MessageType type) => type.TypeName;

    public override string ToString()
    {
        return TypeName;
    }
}