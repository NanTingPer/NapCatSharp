using NapCatSharp.RequestModels;

namespace NapCatSharp.RequestModels.SystemInterfaces;

/// <summary>检查是否可以发送图片</summary>
public class CanSendImage : RequestModelBase
{
    public override string GetEndpoint()
    {
        return "/can_send_image";
    }
}
