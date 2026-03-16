using NapCatSharp.RequestModels;
using System.Text.Json;

namespace NapCatSharp.Utils;

public static class ListExtension
{
    extension(List<IOB11MessageModelFlag> oB11MessageModelFlags)
    {
        public StringContent ToJsonStringContent(JsonSerializerOptions? options = null)
        {
            var jsonValue = ToJson(oB11MessageModelFlags, options);
            return new StringContent(jsonValue, System.Text.Encoding.UTF8, "application/json");
        }

        public string ToJson(JsonSerializerOptions? options = null)
        {
            return JsonSerializer.Serialize(oB11MessageModelFlags, options);
        }
    }
}
