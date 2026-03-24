using System.Text.Json.Serialization;

namespace NapCatSharp.RequestModels.SystemInterfaces;

/// <summary>检查是否可以发送图片响应</summary>
public class CanSendImageResponse : ResponseBaseModel<CanSendImageResponse.DataInfo>
{
    public class DataInfo
    {
        /// <summary>是否可以发送</summary>
        [JsonPropertyName("yes")]
        public bool Yes { get; set; }
    }

    public static implicit operator bool(CanSendImageResponse response) => response.Data?.Yes ?? false;
}
