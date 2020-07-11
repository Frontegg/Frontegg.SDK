using System;
using System.Text.Json;
using Frontegg.SDK.Client.Infa;

namespace Frontegg.SDK.Client
{
    public class FronteggClientOptions
    {
        public TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromSeconds(30);
        
        public IJsonSerializer JsonSerializer { get; set; } = new InnerJsonSerializer(o =>
        {
            o.PropertyNameCaseInsensitive = true;
            o.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });
        public Uri FronteggUrl { get; set; } = new Uri(Constants.FronteggApiUrl);
    }
}