using System.Text.Json.Serialization;

namespace NapCatSharp.RequestModels.SystemInterfaces;

/// <summary>检查是否可以发送语音响应</summary>
public class CanSendRecordResponse : ResponseBaseModel<CanSendRecordResponse.DataInfo>
{
    public class DataInfo
    {
        /// <summary>是否可以发送</summary>
        [JsonPropertyName("yes")]
        public bool Yes { get; set; }
    }
    public static implicit operator bool(CanSendRecordResponse response) => response.Data?.Yes ?? false;
}