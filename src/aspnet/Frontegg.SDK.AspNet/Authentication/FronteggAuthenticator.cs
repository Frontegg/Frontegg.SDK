using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Frontegg.SDK.AspNet.Http;
using Microsoft.Extensions.Logging;

namespace Frontegg.SDK.AspNet.Authentication
{
    internal class FronteggAuthenticator : IFronteggAuthenticator
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<FronteggAuthenticator> _logger;

        public FronteggAuthenticator(IHttpClientFactory httpClientFactory, ILogger<FronteggAuthenticator> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<FronteggAuthenticationResult> Authenticate(IFronteggCredentials fronteggCredentials,
            FronteggOptions options)
        {
            var httpClient = _httpClientFactory.CreateClient(Frontegg.ClientName);
            var json = $@"{{""clientId"": ""{fronteggCredentials.ClientId}"", ""secret"": ""{fronteggCredentials.ApiKey}""}}";

            var content = new StringContent(json);
            content.AddJsonUtf8ContentType();

            try
            {
                var response = await httpClient.PostAsync(options.AuthenticationRelativePath, content)
                    .ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    return await ProcessSuccessfulResponse(response).ConfigureAwait(false);
                }

                return await ProcessFailedResponse(response).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An exception has occured during frontegg authentication process.");
                return FronteggAuthenticationResult.FailedResult(e.Message ?? "");
            }
        }

        private static async Task<FronteggAuthenticationResult> ProcessFailedResponse(HttpResponseMessage response)
        {
            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var webResponse = JsonSerializer.Deserialize<FronteggUnAuthenticationWebResponse>(responseBody,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });
                return FronteggAuthenticationResult.FailedResult($"{webResponse.Error}, {webResponse.Message}");
            }

            var exceptionMessage =
                $"Unable to authenticate user, StatusCode: {response.StatusCode}, Message: {response.ReasonPhrase}, Data: {responseBody}";
            
            throw new FailedAuthorisationsException(exceptionMessage);
        }

        private static async Task<FronteggAuthenticationResult> ProcessSuccessfulResponse(HttpResponseMessage response)
        {
            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var webResponse = JsonSerializer.Deserialize<FronteggSuccessfulAuthenticationResponse>(responseBody,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

            return new FronteggAuthenticationResult(webResponse.Token, webResponse.ExpiresIn);
        }
    }
}