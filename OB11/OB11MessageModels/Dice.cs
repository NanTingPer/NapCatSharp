using NapCatSharp.OB11;
using System.Text.Json.Serialization;

namespace NapCatSharp.OB11.OB11MessageModels;

/// <summary> 骰子消息段 </summary>
public class Dice : OB11MessageModelBase<Dice.OB11MessageDice, Dice>
{
    public class OB11MessageDice
    {
        /// <summary> 骰子结果 </summary>
        [JsonPropertyName("result")]
        public required LongId Result { get; set; }
    }
}
