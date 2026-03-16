using System.Text.Json.Serialization;
using static NapCatSharp.RequestModels.OB11MessageModels.Text;

namespace NapCatSharp.RequestModels.OB11MessageModels;

public class Text : OB11MessageModelBase<OB11MessageText, Text>
{
    public class OB11MessageText
    {
        [JsonPropertyName("text")]
        public required string Text { get; set; }
    }
}
