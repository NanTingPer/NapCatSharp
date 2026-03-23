namespace NapCatSharp.RequestModels.MessageInterfaces;

public class MarkAllAsRead : RequestModelBase
{
    public override string GetEndpoint()
    {
        return "/_mark_all_as_read";
    }
}
