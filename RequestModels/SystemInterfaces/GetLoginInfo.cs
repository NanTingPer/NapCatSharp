using NapCatSharp.RequestModels;

namespace NapCatSharp.RequestModels.SystemInterfaces;

/// <summary>获取登录号信息</summary>
public class GetLoginInfo : RequestModelBase
{
    public override string GetEndpoint()
    {
        return "/get_login_info";
    }
}
