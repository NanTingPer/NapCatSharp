using NapCatSharp.OB11;
using NapCatSharp.RequestModels;
using NapCatSharp.RequestModels.GroupInterfaces;
using System.Text.Json;

namespace NapCatSharp.Core;

public partial class NapCatHttpServer
{
    /// <summary>
    /// 获取群详细信息
    /// </summary>
    /// <param name="msg"> 请求参数 </param>
    /// <returns></returns>
    public Task<GetGroupDetailInfoResponse?> GetGroupDetailInfo(GetGroupDetailInfo msg)
        => Post<GetGroupDetailInfoResponse>(msg);

    /// <summary>
    /// 获取群列表
    /// </summary>
    /// <returns></returns>
    public Task<GetGroupListResponse?> GetGroupList()
        => Post<GetGroupListResponse>(new GetGroupList());

    /// <summary>
    /// 获取群列表
    /// </summary>
    /// <param name="noCache"> 是否不使用缓存 </param>
    /// <returns></returns>
    public Task<GetGroupListResponse?> GetGroupList(bool? noCache)
        => Post<GetGroupListResponse>(new GetGroupList() { NoCache = noCache });

    /// <summary>
    /// 获取群信息
    /// </summary>
    /// <param name="msg"> 请求参数 </param>
    /// <returns></returns>
    public Task<GetGroupInfoResponse?> GetGroupInfo(GetGroupInfo msg)
        => Post<GetGroupInfoResponse>(msg);

    /// <summary>
    /// 获取群成员列表
    /// </summary>
    /// <param name="msg"> 请求参数 </param>
    /// <returns></returns>
    public Task<GetGroupMemberListResponse?> GetGroupMemberList(GetGroupMemberList msg)
        => Post<GetGroupMemberListResponse>(msg);

    /// <summary>
    /// 获取群成员信息
    /// </summary>
    /// <param name="msg"> 请求参数 </param>
    /// <returns></returns>
    public Task<GetGroupMemberInfoResponse?> GetGroupMemberInfo(GetGroupMemberInfo msg)
        => Post<GetGroupMemberInfoResponse>(msg);

    /// <summary>
    /// 处理加群请求
    /// <br/> 使用 <see cref="GetGroupSystemMsg(RequestModels.SystemInterfaces.GetGroupSystemMsg)"/>获取请求Id
    /// <br/> 在接受到<see cref="NapCatHttpSocket.RequestEvent"/>时，会传递<see cref="NapCatSharp.EventPushModels.RequestEvents.AddFriend"/> or <see cref="EventPushModels.RequestEvents.AddGroup"/> 其中包含 flag
    /// </summary>
    /// <param name="msg"> 请求参数 </param>
    /// <returns></returns>
    public Task<SimpleResponseModel?> SetGroupAddRequest(SetGroupAddRequest msg)
        => Post<SimpleResponseModel>(msg);

    /// <summary>
    /// 退出群组
    /// </summary>
    /// <param name="msg"> 请求参数 </param>
    /// <returns></returns>
    public Task<SimpleResponseModel?> SetGroupLeave(SetGroupLeave msg)
        => Post<SimpleResponseModel>(msg);

    /// <summary>
    /// 设置群名称
    /// </summary>
    /// <param name="msg"> 请求参数 </param>
    /// <returns></returns>
    public Task<SimpleResponseModel?> SetGroupName(SetGroupName msg)
        => Post<SimpleResponseModel>(msg);

    /// <summary>
    /// 设置群名片
    /// </summary>
    /// <param name="msg"> 请求参数 </param>
    /// <returns></returns>
    public Task<SimpleResponseModel?> SetGroupCard(SetGroupCard msg)
        => Post<SimpleResponseModel>(msg);

    /// <summary>
    /// 获取群公告
    /// </summary>
    /// <param name="msg"> 请求参数 </param>
    /// <returns></returns>
    public Task<GetGroupNoticeResponse?> GetGroupNotice(GetGroupNotice msg)
        => Post<GetGroupNoticeResponse>(msg);

    /// <summary>
    /// 获取群精华消息
    /// </summary>
    /// <param name="msg"> 请求参数 </param>
    /// <returns></returns>
    public Task<GetEssenceMsgListResponse?> GetEssenceMsgList(GetEssenceMsgList msg)
        => Post<GetEssenceMsgListResponse>(msg);

    /// <summary>
    /// 获取群忽略通知
    /// </summary>
    /// <returns></returns>
    public Task<GetGroupIgnoredNotifiesResponse?> GetGroupIgnoredNotifies()
        => Post<GetGroupIgnoredNotifiesResponse>(new GetGroupIgnoredNotifies());

    /// <summary>
    /// 移出精华消息
    /// </summary>
    /// <param name="msg"> 请求参数 </param>
    /// <returns></returns>
    public Task<SimpleResponseModel?> DeleteEssenceMsg(DeleteEssenceMsg msg)
        => Post<SimpleResponseModel>(msg);

    /// <summary>
    /// 设置精华消息
    /// </summary>
    /// <param name="msg"> 请求参数 </param>
    /// <returns></returns>
    public Task<SimpleResponseModel?> SetEssenceMsg(SetEssenceMsg msg)
        => Post<SimpleResponseModel>(msg);
}
