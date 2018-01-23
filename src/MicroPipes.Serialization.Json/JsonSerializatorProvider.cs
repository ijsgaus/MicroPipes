using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MicroPipes.Serialization.Json
{
    public class JsonSerializatorProvider : TextSerializatorProvider
    {
        private readonly JsonSerializerSettings _settings;

        public JsonSerializatorProvider(JsonSerializerSettings settings = null)
        {
            _settings = settings ?? new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        }

        protected override bool IsMediaTypeSupported(string mediaType)
        {
            return mediaType.Contains("/json") || mediaType.Contains("+json");
        }

        protected override string Serialize(string mediaType, object value)
        {
            return JsonConvert.SerializeObject(value, _settings);
        }

        protected override (bool, object) Deserialize(string mediaType, string data, Type toType)
        {
            try
            {
                return (true, JsonConvert.DeserializeObject(data, toType, _settings));
            }
            catch
            {
                return (false, null);
            }
        }
    }
}