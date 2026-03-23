using NapCatSharp.OB11;
using System.Text.Json.Serialization;

namespace NapCatSharp.OB11.OB11MessageModels;

/// <summary> 联系人消息段 </summary>
public class Contact : OB11MessageModelBase<Contact.OB11MessageContact, Contact>
{
    public class OB11MessageContact
    {
        /// <summary> 联系人类型 </summary>
        [JsonPropertyName("type")]
        public required string Type { get; set; }

        /// <summary> 联系人ID </summary>
        [JsonPropertyName("id")]
        public required StringId Id { get; set; }
    }
}
