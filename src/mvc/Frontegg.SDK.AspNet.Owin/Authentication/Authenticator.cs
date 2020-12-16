using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Frontegg.SDK.AspNet.Owin.Extensions;

namespace Frontegg.SDK.AspNet.Owin.Authentication
{
    internal class Authenticator : IAuthenticator
    {
        private readonly FronteggOptions _options;
        private readonly Uri _authenticationUrl;

        public Authenticator(FronteggOptions options)
        {
            _options = options;
            _authenticationUrl = new UrlBuilder(options.Url)
                .AddPath(options.AuthenticationPath)
                .ToUri();
        }

        public async Task<FronteggAuthenticationState> Authenticate(string clientId, string apiKey)
        {
            var json = $@"{{""clientId"": ""{clientId}"", ""secret"": ""{apiKey}""}}";

            var content = new StringContent(json);
            content.AddJsonUtf8ContentType();
            var ct = new CancellationToken();
            
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.PostAsync(_authenticationUrl.OriginalString, content, ct)
                    .ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    return await ProcessSuccessfulResponse(response)
                        .ConfigureAwait(false);
                }

                return await ProcessFailedResponse(response).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Debug.Fail(e.ToString());
                return FronteggAuthenticationState.FailedResult(e.Message ?? "An error has occured during the authentication process.");
            }
        }

        private static async Task<FronteggAuthenticationState> ProcessFailedResponse(HttpResponseMessage response)
        {
            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var webResponse = JsonSerializer.Deserialize<FronteggUnAuthenticationWebResponse>(responseBody,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });
                return FronteggAuthenticationState.FailedResult($"{webResponse.Error}, {webResponse.Message}");
            }

            var exceptionMessage =
                $"Unable to authenticate user, StatusCode: {response.StatusCode}, Message: {response.ReasonPhrase}, Data: {responseBody}";
            
            throw new FailedAuthorisationsException(exceptionMessage);
        }

        private static async Task<FronteggAuthenticationState> ProcessSuccessfulResponse(HttpResponseMessage response)
        {
            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var webResponse = JsonSerializer.Deserialize<FronteggSuccessfulAuthenticationResponse>(responseBody,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

            return new FronteggAuthenticationState(webResponse.Token, webResponse.ExpiresIn);
        }
    }
}