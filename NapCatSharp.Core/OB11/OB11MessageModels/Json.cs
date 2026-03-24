using NapCatSharp.OB11;
using System.Text.Json.Serialization;

namespace NapCatSharp.OB11.OB11MessageModels;

/// <summary> JSON消息段 </summary>
public class Json : OB11MessageModelBase<Json.OB11MessageJson, Json>
{
    public class OB11MessageJson
    {
        /// <summary> JSON数据 </summary>
        [JsonPropertyName("data")]
        public required object Data { get; set; }

        /// <summary> 配置 </summary>
        [JsonPropertyName("config")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public JsonConfig? Config { get; set; }

        /// <summary> JSON配置 </summary>
        public class JsonConfig
        {
            /// <summary> Token </summary>
            [JsonPropertyName("token")]
            public required string Token { get; set; }
        }
    }
}
