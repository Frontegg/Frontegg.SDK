using System;
using Frontegg.SDK.Client.Infa;

namespace Frontegg.SDK.Client
{
    internal interface IClientContext
    {
        Uri FronteggApiUrl { get; }
        IRestClient RestClient { get; set; }
        IJsonSerializer JsonSerializer { get; set; }
    }
}