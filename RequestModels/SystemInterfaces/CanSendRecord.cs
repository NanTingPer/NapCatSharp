using NapCatSharp.RequestModels;

namespace NapCatSharp.RequestModels.SystemInterfaces;

/// <summary>检查是否可以发送语音</summary>
public class CanSendRecord : RequestModelBase
{
    public override string GetEndpoint()
    {
        return "/can_send_record";
    }
}
