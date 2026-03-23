using NapCatSharp.RequestModels;

namespace NapCatSharp.RequestModels.SystemInterfaces;

/// <summary>获取CSRF Token</summary>
public class GetCsrfToken : RequestModelBase
{
    public override string GetEndpoint()
    {
        return "/get_csrf_token";
    }
}
