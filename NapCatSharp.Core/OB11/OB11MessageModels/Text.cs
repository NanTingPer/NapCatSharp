using NapCatSharp.OB11;
using System.Text.Json.Serialization;
using static NapCatSharp.OB11.OB11MessageModels.Text;

namespace NapCatSharp.OB11.OB11MessageModels;

public class Text : OB11MessageModelBase<OB11MessageText, Text>
{
    public class OB11MessageText
    {
        [JsonPropertyName("text")]
        public required string Text { get; set; }
    }

    public override string ToString()
    {
        return Data.Text;
    }

    public static implicit operator string(Text text) => text.Data.Text;
}
