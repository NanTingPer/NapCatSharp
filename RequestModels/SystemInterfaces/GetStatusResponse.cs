using System.Text.Json.Serialization;

namespace NapCatSharp.RequestModels.SystemInterfaces;

/// <summary>获取运行状态响应</summary>
public class GetStatusResponse : ResponseBaseModel<GetStatusResponse.DataInfo>
{
    public class DataInfo
    {
        /// <summary>是否在线</summary>
        [JsonPropertyName("online")]
        public bool Online { get; set; }

        /// <summary>状态是否良好</summary>
        [JsonPropertyName("good")]
        public bool Good { get; set; }

        /// <summary>统计信息</summary>
        [JsonPropertyName("stat")]
        public object Stat { get; set; } = new();
    }
}
