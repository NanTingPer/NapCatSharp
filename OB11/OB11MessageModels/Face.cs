using NapCatSharp.OB11;
using System.Text.Json.Serialization;
using static NapCatSharp.OB11.OB11MessageModels.Face;

namespace NapCatSharp.OB11.OB11MessageModels;

public class Face : OB11MessageModelBase<OB11MessageFace, Face>
{
    public class OB11MessageFace
    {
        /// <summary> 表情ID </summary>
        [JsonPropertyName("id")]
        public required StringId Id { get; set; }

        /// <summary> 结果ID </summary>
        [JsonPropertyName("resultId")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public StringId ResultId { get; set; }

        /// <summary> 连击数 </summary>
        [JsonPropertyName("chainCount")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public LongId ChainCount { get; set; }
    }
}
