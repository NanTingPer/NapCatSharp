using NapCatSharp.OB11;
using NapCatSharp.OB11.OB11MessageModels;
using NapCatSharp.RequestModels;
using NapCatSharp.RequestModels.GroupInterfaces;
using NapCatSharp.RequestModels.MessageInterfaces;
using NapCatSharp.RequestModels.SystemInterfaces;

namespace NapCatSharp.Core;

public class NapCatHttpServerCN : IDisposable
{
    private readonly NapCatHttpServer _server;

    public Uri 地址 { get => _server.Uri; set => _server.Uri = value; }
    public string 密码 { get => _server.Password; set => _server.Password = value; }

    public NapCatHttpServerCN(Uri uri, string password, HttpClient? httpClient = null)
    {
        _server = new NapCatHttpServer(uri, password, httpClient);
    }
    
    public void Dispose()
    {
        _server.Dispose();
    }

    public Task<SimpleResponseModel?> 处理可疑好友申请(SetDoubtFriendsAddRequest request)
        => _server.SetDoubtFriendsAddRequest(request);

    public Task<GetDoubtFriendsAddRequestResponse?> 获取可疑好友申请列表(GetDoubtFriendsAddRequest request)
        => _server.GetDoubtFriendsAddRequest(request);

    public Task<GetLoginInfoResponse?> 获取登录号信息()
        => _server.GetLoginInfo();

    public Task<GetVersionInfoResponse?> 获取版本信息()
        => _server.GetVersionInfo();

    public Task<CanSendRecordResponse?> 检查是否可发送语音()
        => _server.CanSendRecord();

    public Task<CanSendImageResponse?> 检查是否可发送图片()
        => _server.CanSendImage();

    public Task<GetStatusResponse?> 获取运行状态()
        => _server.GetStatus();

    public Task<GetCsrfTokenResponse?> 获取CSRF令牌()
        => _server.GetCsrfToken();

    public Task<GetCredentialsResponse?> 获取登录凭证(GetCredentials request)
        => _server.GetCredentials(request);

    public Task<SimpleResponseModel?> 获取Packet服务状态()
        => _server.NcGetPacketStatus();

    public Task<SimpleResponseModel?> 重启服务()
        => _server.SetRestart();

    public Task<GetGroupSystemMsgResponse?> 获取群系统消息(GetGroupSystemMsg request)
        => _server.GetGroupSystemMsg(request);

    public Task<SimpleResponseModel?> 清理缓存()
        => _server.CleanCache();

    public Task<GetGroupDetailInfoResponse?> 获取群详细信息(GetGroupDetailInfo msg)
        => _server.GetGroupDetailInfo(msg);

    public Task<GetGroupListResponse?> 获取群列表()
        => _server.GetGroupList();

    public Task<GetGroupListResponse?> 获取群列表(bool? noCache)
        => _server.GetGroupList(noCache);

    public Task<GetGroupInfoResponse?> 获取群信息(GetGroupInfo msg)
        => _server.GetGroupInfo(msg);

    public Task<GetGroupMemberListResponse?> 获取群成员列表(GetGroupMemberList msg)
        => _server.GetGroupMemberList(msg);

    public Task<GetGroupMemberInfoResponse?> 获取群成员信息(GetGroupMemberInfo msg)
        => _server.GetGroupMemberInfo(msg);

    public Task<SimpleResponseModel?> 处理加群请求(SetGroupAddRequest msg)
        => _server.SetGroupAddRequest(msg);

    public Task<SimpleResponseModel?> 退出群组(SetGroupLeave msg)
        => _server.SetGroupLeave(msg);

    public Task<SimpleResponseModel?> 设置群名称(SetGroupName msg)
        => _server.SetGroupName(msg);

    public Task<SimpleResponseModel?> 设置群名片(SetGroupCard msg)
        => _server.SetGroupCard(msg);

    public Task<GetGroupNoticeResponse?> 获取群公告(GetGroupNotice msg)
        => _server.GetGroupNotice(msg);

    public Task<GetEssenceMsgListResponse?> 获取群精华消息(GetEssenceMsgList msg)
        => _server.GetEssenceMsgList(msg);

    public Task<GetGroupIgnoredNotifiesResponse?> 获取群忽略通知()
        => _server.GetGroupIgnoredNotifies();

    public Task<SimpleResponseModel?> 移出精华消息(DeleteEssenceMsg msg)
        => _server.DeleteEssenceMsg(msg);

    public Task<SimpleResponseModel?> 设置精华消息(SetEssenceMsg msg)
        => _server.SetEssenceMsg(msg);

    public Task<SendMsgResponse?> 发送私聊文本(string text, LongId userid)
        => _server.SendPrivateTextMsg(text, userid);

    public Task<SendMsgResponse?> 发送私聊消息(List<IOB11MessageModelFlag> msgs, LongId userId)
        => _server.SendPrivateMsg(msgs, userId);

    public Task<SendMsgResponse?> 发送群聊消息(List<IOB11MessageModelFlag> msgs, LongId groupId)
        => _server.SendGroupMsg(msgs, groupId);

    public Task<SendMsgResponse?> 发送消息(SendMsg msg)
        => _server.SendMsg(msg);

    public Task<SimpleResponseModel?> 标记私聊消息为已读(MarkPrivateMsgAsRead markRead)
        => _server.MarkPrivateMsgAsRead(markRead);

    public Task<SimpleResponseModel?> 标记群消息为已读(MarkGroupMsgAsRead markRead)
        => _server.MarkGroupMsgAsRead(markRead);

    public Task<SimpleResponseModel?> 撤回消息(LongId msgId)
        => _server.DeleteMsg(msgId);

    public Task<SimpleResponseModel?> 撤回消息(DeleteMsg msg)
        => _server.DeleteMsg(msg);

    public Task<SimpleResponseModel?> 转发单条消息到私聊(ForwardFriendSingleMsg msg)
        => _server.ForwardFriendSingleMsg(msg);

    public Task<SimpleResponseModel?> 转发单条消息到群聊(ForwardGroupSingleMsg msg)
        => _server.ForwardGroupSingleMsg(msg);

    public Task<SimpleResponseModel?> 标记所有消息为已读()
        => _server.MarkAllAsRead();

    public Task<SimpleResponseModel?> 发送合并转发消息(SendForwardMsg msg)
        => _server.SendForward(msg);

    public Task<SimpleResponseModel?> 发送合并转发消息到群聊(LongId groupId, List<Node> message)
        => _server.SendForwardToGroup(groupId, message);

    public Task<SimpleResponseModel?> 发送合并转发消息到私聊(LongId userId, List<Node> message)
        => _server.SendForwardToPrivate(userId, message);

    public Task<HttpResponseMessage> 发送<TRequestModel>(TRequestModel msg)
        where TRequestModel : RequestModelBase
        => _server.Send(msg);

    public Task<GetMsgResponse> 获取消息详细信息(LongId msgId, Action<Exception>? errorCallback = null)
        => _server.GetMsgInfo(msgId, errorCallback);
}
