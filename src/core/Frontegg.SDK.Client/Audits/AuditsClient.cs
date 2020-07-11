using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Frontegg.SDK.Client.Extensions;
using Frontegg.SDK.Client.Infa;
using Frontegg.SDK.Client.Net;

namespace Frontegg.SDK.Client.Audits
{
    internal class AuditsClient : IAuditsClient
    {
        private readonly string _auditsUrl;
        private readonly IRestClient _restClient;
        private readonly IJsonSerializer _jsonSerializer;

        internal AuditsClient(FronteggClientOptions options, IRestClient restClient) 
            : this(options.FronteggUrl, restClient, options.JsonSerializer)
        { }

        internal AuditsClient(Uri apiUrl, IRestClient restClient, IJsonSerializer jsonSerializer)
        {
            _auditsUrl = apiUrl.ToUrlBuilder().WithPath(Constants.AuditsUrl).ToString();
            _restClient = restClient;
            _jsonSerializer = jsonSerializer;
        }
        
        public async Task SendAudit(string tenantId, object audit)
        {
            if (audit == null) throw new ArgumentNullException(nameof(audit));

            var serializedObject = audit.IsString() ? (string)audit : _jsonSerializer.Serialize(audit);
            
            var message = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_auditsUrl),
                Content = new StringContent(serializedObject)
            };
            message.Content.AddJsonUtf8ContentType();
            
            message.AddTenantIdHeader(tenantId);
            
            var response = await _restClient.Send(message, new CancellationToken())
                .ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
                return;

            throw new FronteggHttpException(response.StatusCode, response.ReasonPhrase);
        }
    }
}