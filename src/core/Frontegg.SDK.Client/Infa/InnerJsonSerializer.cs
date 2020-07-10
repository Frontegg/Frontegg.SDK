using System;
using System.Text.Json;

namespace Frontegg.SDK.Client.Infa
{
    internal class InnerJsonSerializer : IJsonSerializer
    {
        private readonly Action<JsonSerializerOptions> _jsonSerializationOptions;

        public InnerJsonSerializer(Action<JsonSerializerOptions> jsonSerializationOptions = null)
        {
            _jsonSerializationOptions = jsonSerializationOptions;
        }

        public string Serialize(object obj)
        {
            var options = GetOptions();
            var result = JsonSerializer.Serialize(obj, options);
            return result;
        }

        public T Deserialize<T>(string json)
        {
            var options = GetOptions();
            var result = JsonSerializer.Deserialize<T>(json, options);
            return result;
        }

        private JsonSerializerOptions GetOptions()
        {
            var options = new JsonSerializerOptions();
            _jsonSerializationOptions?.Invoke(options);
            return options;
        }
    }
}