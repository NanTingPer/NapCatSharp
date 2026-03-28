using System.Text.Json.Serialization;

namespace NapCatSharp.Mod.Controllers;

public record class ControllerResult<TData>
    where TData : class
{
    [JsonPropertyName("code")]
    public int Code { get; set; } = 200;
    [JsonPropertyName("errorMsg")]
    public string ErrorMsg { get; set; } = string.Empty;
    [JsonPropertyName("data")]
    public TData? Data { get; set; } = null;
}

public record class SimpleControllerResult : ControllerResult<object>
{

}