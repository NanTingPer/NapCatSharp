using NapCatSharp.RequestModels;

namespace NapCatSharp.RequestModels.GroupInterfaces;

/// <summary>获取群忽略通知</summary>
public class GetGroupIgnoredNotifies : RequestModelBase
{
    public override string GetEndpoint()
    {
        return "/get_group_ignored_notifies";
    }
}
