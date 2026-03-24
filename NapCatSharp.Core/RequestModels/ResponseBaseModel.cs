using NapCatSharp.JsonConverter;
using System.Text.Json.Serialization;

namespace NapCatSharp.RequestModels;

public abstract class ResponseBaseModel<TData>
    where TData : class
{
    [JsonPropertyName("status")]
    public StatusEnum Status { get; set; }

    [JsonPropertyName("retcode")]
    public long Retcode { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary> 提示 </summary>
    [JsonPropertyName("wording")]
    public string Wording { get; set; } = string.Empty;

    [JsonPropertyName("data")]
    public TData? Data { get; set; } = null;

    /// <summary>
    /// stream-action<br/>
    /// normal-action
    /// </summary>
    [JsonPropertyName("stream")]
    public string Stream { get; set; } = "normal-action";
}

[JsonConverter(typeof(StatusEnumConverter))]
public enum StatusEnum
{
    ok,
    failed
}

public class StatusEnumConverter : EnumJsonConverter<StatusEnum>{}

public class SimpleResponseModel
{
    [JsonPropertyName("status")]
    public StatusEnum Status { get; set; }

    [JsonPropertyName("retcode")]
    public long Retcode { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary> 提示 </summary>
    [JsonPropertyName("wording")]
    public string Wording { get; set; } = string.Empty;

    /// <summary>
    /// stream-action<br/>
    /// normal-action
    /// </summary>
    [JsonPropertyName("stream")]
    public string Stream { get; set; } = "normal-action";
}
