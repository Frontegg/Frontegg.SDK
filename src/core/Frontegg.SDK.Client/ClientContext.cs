using System;
using Frontegg.SDK.Client.Infa;

namespace Frontegg.SDK.Client
{
    internal class ClientContext: IClientContext
    {
        public Uri FronteggApiUrl { get; set; }
        public IRestClient RestClient { get; set; }
        public IJsonSerializer JsonSerializer { get; set; }
    }
}