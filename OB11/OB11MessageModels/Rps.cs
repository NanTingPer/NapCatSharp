using NapCatSharp.OB11;
using System.Text.Json.Serialization;

namespace NapCatSharp.OB11.OB11MessageModels;

/// <summary> 猜拳消息段 </summary>
public class Rps : OB11MessageModelBase<Rps.OB11MessageRps, Rps>
{
    public class OB11MessageRps
    {
        /// <summary> 猜拳结果 </summary>
        [JsonPropertyName("result")]
        public required LongId Result { get; set; }
    }
}
