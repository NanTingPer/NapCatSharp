using NapCatSharp.OB11;
using NapCatSharp.OB11.OB11MessageModels;
using NapCatSharp.RequestModels;
using NapCatSharp.RequestModels.MessageInterfaces;
using System.Text.Json;

namespace NapCatSharp.Core;

public partial class NapCatHttpServer
{
    /// <summary>
    /// 发送私聊文本
    /// </summary>
    /// <param name="text"> 文本内容 </param>
    /// <param name="userid"> 用户id </param>
    /// <returns></returns>
    public Task<SendMsgResponse?> SendPrivateTextMsg(string text, LongId userid)
    {
        var msg = new SendPrivateMsg()
        {
            UserId = userid,
            Message = [
                new Text()
                {
                    Data = new Text.OB11MessageText()
                    {
                        Text = text
                    }
                }
            ],
        };

        return Post<SendMsgResponse>(msg);
    }

    /// <summary> 发送私聊消息 </summary>
    public Task<SendMsgResponse?> SendPrivateMsg(List<IOB11MessageModelFlag> msgs, LongId userId)
    {
        var msg = new SendPrivateMsg()
        {
            UserId = userId,
            Message = msgs
        };
        return Post<SendMsgResponse>(msg);
    }
    public Task<SendMsgResponse?> SendGroupMsg(List<IOB11MessageModelFlag> msgs, LongId groupId)
    {
        var msg = new SendMsg()
        {
            MessageType = RequestModels.MessageInterfaces.SendMsg.MessageTypeEnum.group,
            Message = msgs,
            GroupId = groupId,
        };
        return Post<SendMsgResponse>(msg);
    }

    /// <summary> 根据<see cref="SendMsg.MessageType"/>确定是群聊消息还是私聊消息 </summary>
    public Task<SendMsgResponse?> SendMsg(SendMsg msg)
        => Post<SendMsgResponse>(msg);

    /// <summary>
    /// 标记私聊消息为已读
    /// </summary>
    /// <returns></returns>
    public Task<SimpleResponseModel?> MarkPrivateMsgAsRead(MarkPrivateMsgAsRead markRead)
        => Post<SimpleResponseModel>(markRead);

    /// <summary>
    /// 标记群消息为已读
    /// </summary>
    /// <returns></returns>
    public Task<SimpleResponseModel?> MarkGroupMsgAsRead(MarkGroupMsgAsRead markRead)
        => Post<SimpleResponseModel>(markRead);

    /// <summary> 撤回消息 </summary>
    public Task<SimpleResponseModel?> DeleteMsg(LongId msgId)
        => Post<SimpleResponseModel>(new DeleteMsg(){ MessageId = msgId });

    /// <summary> 撤回消息 </summary>
    public Task<SimpleResponseModel?> DeleteMsg(DeleteMsg msg) 
        => Post<SimpleResponseModel>(msg);

    /// <summary> 转发单条消息到私聊 </summary>
    public Task<SimpleResponseModel?> ForwardFriendSingleMsg(ForwardFriendSingleMsg msg)
        => Post<SimpleResponseModel>(msg);

    /// <summary> 转发单条消息到群聊 </summary>
    public Task<SimpleResponseModel?> ForwardGroupSingleMsg(ForwardGroupSingleMsg msg)
        => Post<SimpleResponseModel>(msg);

    /// <summary> 标记所有消息为已读 </summary>
    public Task<SimpleResponseModel?> MarkAllAsRead()
        => Post<SimpleResponseModel>(new MarkAllAsRead());

    /// <summary> 发送合并转发消息 </summary>
    public Task<SimpleResponseModel?> SendForward(SendForwardMsg msg)
        => Post<SimpleResponseModel>(msg);

    /// <summary> 发送合并转发消息到群聊 </summary>
    public Task<SimpleResponseModel?> SendForwardToGroup(LongId groupId, List<Node> message)
    {
        var msg = new SendForwardMsg()
        {
            GroupId = groupId,
            MessageType = MessageType.group,
            Message = message
        };
        return Post<SimpleResponseModel>(msg);
    }

    /// <summary> 发送合并转发消息到私聊 </summary>
    public Task<SimpleResponseModel?> SendForwardToPrivate(LongId userId, List<Node> message)
    {
        var msg = new SendForwardMsg()
        {
            UserId = userId,
            MessageType = MessageType.@private,
            Message = message
        };
        return Post<SimpleResponseModel>(msg);
    }

    public Task<HttpResponseMessage> Send<TRequestModel>(TRequestModel msg)
        where TRequestModel : RequestModelBase
        => Post(msg);

    /// <summary> 根据消息 ID 获取消息详细信息 </summary>
    public async Task<GetMsgResponse> GetMsgInfo(LongId msgId, Action<Exception>? errorCallback = null)
    {
        var response = await Post(new GetMsg(){ MessageId = msgId });
        try {
            var respV = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<GetMsgResponse>(respV) ?? new GetMsgResponse() { Status = RequestModels.StatusEnum.failed };
        } catch(Exception e) {
            errorCallback?.Invoke(e);
            return new GetMsgResponse() { Status = RequestModels.StatusEnum.failed };
        }
    }
}
