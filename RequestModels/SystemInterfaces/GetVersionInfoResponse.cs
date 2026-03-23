using System.Text.Json.Serialization;

namespace NapCatSharp.RequestModels.SystemInterfaces;

/// <summary>获取版本信息响应</summary>
public class GetVersionInfoResponse : ResponseBaseModel<GetVersionInfoResponse.DataInfo>
{
    public class DataInfo
    {
        /// <summary>应用名称</summary>
        [JsonPropertyName("app_name")]
        public string AppName { get; set; } = string.Empty;

        /// <summary>协议版本</summary>
        [JsonPropertyName("protocol_version")]
        public string ProtocolVersion { get; set; } = string.Empty;

        /// <summary>应用版本</summary>
        [JsonPropertyName("app_version")]
        public string AppVersion { get; set; } = string.Empty;
    }
}
