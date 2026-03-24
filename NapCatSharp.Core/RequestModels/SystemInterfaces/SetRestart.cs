using NapCatSharp.RequestModels;

namespace NapCatSharp.RequestModels.SystemInterfaces;

/// <summary>重启服务</summary>
public class SetRestart : RequestModelBase
{
    public override string GetEndpoint()
    {
        return "/set_restart";
    }
}
