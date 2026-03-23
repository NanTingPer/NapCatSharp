using NapCatSharp.RequestModels;

namespace NapCatSharp.RequestModels.SystemInterfaces;

/// <summary>获取版本信息</summary>
public class GetVersionInfo : RequestModelBase
{
    public override string GetEndpoint()
    {
        return "/get_version_info";
    }
}
