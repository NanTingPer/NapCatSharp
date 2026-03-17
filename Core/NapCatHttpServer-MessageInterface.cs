using NapCatSharp.OB11;
using NapCatSharp.OB11.OB11MessageModels;
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
    public Task<HttpResponseMessage> SendPrivateTextMsg(string text, LongId userid)
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

        return Post(msg);
    }

    /// <summary> 发送私聊消息 </summary>
    public Task<HttpResponseMessage> SendPrivateMsg(List<IOB11MessageModelFlag> msgs, LongId userId)
    {
        var msg = new SendPrivateMsg()
        {
            UserId = userId,
            Message = msgs
        };
        return Post(msg);
    }
    public Task<HttpResponseMessage> SendGroupMsg(List<IOB11MessageModelFlag> msgs, LongId groupId)
    {
        var msg = new SendMsg()
        {
            MessageType = RequestModels.MessageInterfaces.SendMsg.MessageTypeEnum.group,
            Message = msgs,
            GroupId = groupId,
        };
        return Post(msg);
    }

    /// <summary> 根据<see cref="SendMsg.MessageType"/>确定是群聊消息还是私聊消息 </summary>
    public Task<HttpResponseMessage> SendMsg(SendMsg msg)
        => Post(msg);

    /// <summary>
    /// 标记私聊消息为已读
    /// </summary>
    /// <returns></returns>
    public Task<HttpResponseMessage> MarkPrivateMsgAsRead(MarkPrivateMsgAsRead markRead)
        => Post(markRead);

    /// <summary>
    /// 标记群消息为已读
    /// </summary>
    /// <returns></returns>
    public Task<HttpResponseMessage> MarkGroupMsgAsRead(MarkGroupMsgAsRead markRead)
        => Post(markRead);

    /// <summary> 撤回消息 </summary>
    public Task<HttpResponseMessage> DeleteMsg(LongId msgId)
        => Post(new DeleteMsg(){ MessageId = msgId });

    /// <summary> 撤回消息 </summary>
    public Task<HttpResponseMessage> DeleteMsg(DeleteMsg msg) 
        => Post(msg);

    /// <summary> 转发单条消息到私聊 </summary>
    public Task<HttpResponseMessage> ForwardFriendSingleMsg(ForwardFriendSingleMsg msg)
        => Post(msg);

    /// <summary> 转发单条消息到群聊 </summary>
    public Task<HttpResponseMessage> ForwardGroupSingleMsg(ForwardGroupSingleMsg msg)
        => Post(msg);

    /// <summary> 标记所有消息为已读 </summary>
    public Task<HttpResponseMessage> MarkAllAsRead()
        => Post(new MarkAllAsRead());

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
