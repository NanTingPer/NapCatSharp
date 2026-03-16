using NapCatSharp.OB11;
using System.Text.Json.Serialization;

namespace NapCatSharp.OB11.OB11MessageModels;

/// <summary> @消息段 </summary>
public class At : OB11MessageModelBase<At.OB11MessageAt, At>
{
    public class OB11MessageAt
    {
        /// <summary> QQ号或all </summary>
        [JsonPropertyName("qq")]
        public required StringId QQ { get; set; }  // 改为 StringId

        /// <summary> 显示名称 </summary>
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Name { get; set; }
    }
}