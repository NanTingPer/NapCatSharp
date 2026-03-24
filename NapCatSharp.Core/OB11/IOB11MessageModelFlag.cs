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

    public static TOB11MessageModel Create<TOB11MessageModel, TData>(TData data)
        where TOB11MessageModel : OB11MessageModelBase<TData, TOB11MessageModel>
        where TData : class
    {
        return OB11MessageModelBase<TData, TOB11MessageModel>.Create(data);
    }

    public static Text CreateText(string text)
    {
        return new Text(){ Data = new Text.OB11MessageText(){ Text = text } };
    }

    public static Face CreateFace(Face.OB11MessageFace face)
    {
        return new Face(){ Data = face };
    }

    public static Image CreateImage(Image.OB11MessageImage image)
    {
        return new Image(){ Data = image };
    }

    public static MFace CreateMFace(MFace.OB11MessageMFace mface)
    {
        return new MFace(){ Data = mface };
    }

    public static Reply CreateReply(Reply.OB11MessageReply reply)
    {
        return Create<Reply, Reply.OB11MessageReply>(reply);
    }
}