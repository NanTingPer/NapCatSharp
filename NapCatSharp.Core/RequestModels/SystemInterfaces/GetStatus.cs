using NapCatSharp.RequestModels;

namespace NapCatSharp.RequestModels.SystemInterfaces;

/// <summary>获取运行状态</summary>
public class GetStatus : RequestModelBase
{
    public override string GetEndpoint()
    {
        return "/get_status";
    }
}
