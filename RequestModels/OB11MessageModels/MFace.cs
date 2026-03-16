using System.Text.Json.Serialization;

namespace NapCatSharp.RequestModels.OB11MessageModels;
/// <summary> 商城表情消息段 </summary>
public class MFace : OB11MessageModelBase<MFace.OB11MessageMFace, MFace>
{
    public class OB11MessageMFace
    {
        /// <summary> 表情包ID </summary>
        [JsonPropertyName("emoji_package_id")]
        public required LongId EmojiPackageId { get; set; }

        /// <summary> 表情ID </summary>
        [JsonPropertyName("emoji_id")]
        public required string EmojiId { get; set; }

        /// <summary> 表情key </summary>
        [JsonPropertyName("key")]
        public required StringId Key { get; set; }

        /// <summary> 表情摘要 </summary>
        [JsonPropertyName("summary")]
        public required string Summary { get; set; }
    }
}