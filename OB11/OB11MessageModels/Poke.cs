using NapCatSharp.OB11;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NapCatSharp.OB11.OB11MessageModels;

/// <summary> 戳一戳消息段 </summary>
public class Poke : OB11MessageModelBase<Poke.OB11MessagePoke, Poke>
{
    public class OB11MessagePoke
    {
        [JsonPropertyName("type")]
        public required string Type { get; set; }
        [JsonPropertyName("id")]
        public required StringId Id { get; set; }  // 改为 StringId
    }
}
