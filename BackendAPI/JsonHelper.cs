using System.Text.Json;
using Newtonsoft.Json;

namespace BackendAPI
{
    public static class JsonHelper
    {
        public static string Serialize(object obj)
        {
            return System.Text.Json.JsonSerializer.Serialize(obj);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
