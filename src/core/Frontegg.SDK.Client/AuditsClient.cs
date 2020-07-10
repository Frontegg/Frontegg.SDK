using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Frontegg.SDK.Client.Infa;
using Frontegg.SDK.Client.Net;

namespace Frontegg.SDK.Client
{
    internal class AuditsClient : IAuditsClient
    {
        private readonly string _auditsUrl;
        private readonly IRestClient _restClient;

        public AuditsClient(string baseUrl, IClientContext context):
            this(baseUrl, context.RestClient)
        {
        }

        internal AuditsClient(string baseUrl, IRestClient restClient)
        {
            _auditsUrl = new UrlBuilder(baseUrl).WithPath(Constants.AuditsUrl).ToString();
            _restClient = restClient;
        }
        
        public async Task SendAudits(string tenantId, IEnumerable<object> audits)
        {
            if (audits == null) throw new ArgumentNullException(nameof(audits));
            if(!audits.Any()) return;

            var message = new HttpRequestMessage {Method = HttpMethod.Post, RequestUri = new Uri(_auditsUrl)};
            message.AddTenantIdHeader(tenantId);
            
            var response = await _restClient.Send(message, new CancellationToken())
                .ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
                return;

            throw new FronteggHttpException(response.StatusCode, response.ReasonPhrase);
        }
    }

    
}