using NapCatSharp.OB11;
using NapCatSharp.RequestModels;
using NapCatSharp.RequestModels.SystemInterfaces;

namespace NapCatSharp.Core;

public partial class NapCatHttpServer
{
    /// <summary>处理可疑好友申请</summary>
    public Task<SimpleResponseModel?> SetDoubtFriendsAddRequest(SetDoubtFriendsAddRequest request)
        => Post<SimpleResponseModel>(request);

    /// <summary>获取可疑好友申请列表</summary>
    public Task<GetDoubtFriendsAddRequestResponse?> GetDoubtFriendsAddRequest(GetDoubtFriendsAddRequest request)
        => Post<GetDoubtFriendsAddRequestResponse>(request);

    /// <summary>获取登录号信息</summary>
    public Task<GetLoginInfoResponse?> GetLoginInfo()
        => Post<GetLoginInfoResponse>(new GetLoginInfo());

    /// <summary>获取版本信息</summary>
    public Task<GetVersionInfoResponse?> GetVersionInfo()
        => Post<GetVersionInfoResponse>(new GetVersionInfo());

    /// <summary>检查是否可以发送语音</summary>
    public Task<CanSendRecordResponse?> CanSendRecord()
        => Post<CanSendRecordResponse>(new CanSendRecord());

    /// <summary>检查是否可以发送图片</summary>
    public Task<CanSendImageResponse?> CanSendImage()
        => Post<CanSendImageResponse>(new CanSendImage());

    /// <summary>获取运行状态</summary>
    public Task<GetStatusResponse?> GetStatus()
        => Post<GetStatusResponse>(new GetStatus());

    /// <summary>获取CSRF Token</summary>
    public Task<GetCsrfTokenResponse?> GetCsrfToken()
        => Post<GetCsrfTokenResponse>(new GetCsrfToken());

    /// <summary>获取登录凭证</summary>
    public Task<GetCredentialsResponse?> GetCredentials(GetCredentials request)
        => Post<GetCredentialsResponse>(request);

    /// <summary>获取底层Packet服务的运行状态</summary>
    public Task<SimpleResponseModel?> NcGetPacketStatus()
        => Post<SimpleResponseModel>(new NcGetPacketStatus());

    /// <summary>重启服务</summary>
    public Task<SimpleResponseModel?> SetRestart()
        => Post<SimpleResponseModel>(new SetRestart());

    /// <summary>获取群系统消息</summary>
    public Task<GetGroupSystemMsgResponse?> GetGroupSystemMsg(GetGroupSystemMsg request)
        => Post<GetGroupSystemMsgResponse>(request);

    /// <summary>清理缓存</summary>
    public Task<SimpleResponseModel?> CleanCache()
        => Post<SimpleResponseModel>(new CleanCache());
}
