using NapCatSharp.OB11;
using System.Text.Json;
using static NapCatSharp.Core.NapCatHttpSocket;

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

    extension(List<Action<EventMessageData>> events)
    {
        public void operator += (Action<EventMessageData>? ar2)
        {
            if(ar2 == null) return;
            lock (events) {
                try {
                    events.Add(ar2);
                } catch {}
            }
        }

        public void operator -= (Action<EventMessageData>? ar2)
        {
            if(ar2 == null) return;
            lock (events) {
                try {
                    events.Remove(ar2);
                } catch {}
            }
        }
    }
}
