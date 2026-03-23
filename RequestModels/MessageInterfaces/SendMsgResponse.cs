using System.Text.Json.Serialization;

namespace NapCatSharp.RequestModels.MessageInterfaces;

public class SendMsgResponse : ResponseBaseModel<SendMsgResponse.RespData>
{
    public class RespData
    {
        [JsonPropertyName("message_id")]
        public LongId MessageId { get; set; }
    }

    [JsonIgnore]
    public LongId MessageId => Data?.MessageId ?? default;
}
