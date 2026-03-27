using System.Text.Json.Serialization;

namespace NapCatSharp.Mod.Controllers;

public class NotFoundJson
{
    public readonly static NotFoundJson Not = new NotFoundJson();
    private NotFoundJson()
    {
    }
    [JsonPropertyName("statu")]
    public int Statu { get; set; } = 404;
    public object Data { get; set; } = "404 NotFound";
}
