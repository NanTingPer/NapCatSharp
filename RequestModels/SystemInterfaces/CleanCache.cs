using NapCatSharp.RequestModels;

namespace NapCatSharp.RequestModels.SystemInterfaces;

/// <summary>清理缓存</summary>
public class CleanCache : RequestModelBase
{
    public override string GetEndpoint()
    {
        return "/clean_cache";
    }
}
