using NapCatSharp.RequestModels;

namespace NapCatSharp.RequestModels.SystemInterfaces;

/// <summary>获取底层Packet服务的运行状态</summary>
public class NcGetPacketStatus : RequestModelBase
{
    public override string GetEndpoint()
    {
        return "/nc_get_packet_status";
    }
}
