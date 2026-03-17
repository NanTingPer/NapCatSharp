using NapCatSharp.JsonConverter;
using System.Text.Json.Serialization;
using NapCatSharp.OB11.OB11MessageModels;

namespace NapCatSharp.OB11;

/// <summary>
/// <see cref="At"/>  at消息
/// <br/><see cref="Text"/>  文本消息
/// <br/><see cref="Face"/>  表情消息
/// <br/><see cref="MFace"/> 商城表情
/// <br/><see cref="Image"/> 图片消息
/// <br/><see cref="Node"/>  合并转发节点
/// <br/><see cref="Poke"/>  戳一戳
/// <br/><see cref="Reply"/> 回复消息
/// <br/><see cref="Forward"/> 合并消息
/// </summary>
[JsonConverter(typeof(OB11MessageModelFlagConver))]
public interface IOB11MessageModelFlag
{
    OB11MessageType GetMessageType();
}