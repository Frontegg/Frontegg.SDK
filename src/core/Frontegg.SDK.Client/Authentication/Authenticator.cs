using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Frontegg.SDK.Client.Net;

namespace Frontegg.SDK.Client.Authentication
{
    internal class Authenticator : IAuthenticator
    {
        private readonly Uri _authenticationUrl;

        public Authenticator(string authenticationUrl)
        {
            _authenticationUrl = new Uri(authenticationUrl);
        }

        public Authenticator(Uri apiUrl)
        {
            _authenticationUrl = apiUrl.ToUrlBuilder().WithPath(Constants.AuthorizationPath).ToUri();
        }
        
        public async Task<FronteggAuthenticationState> Authenticate(IFronteggCredentials fronteggCredentials)
        {
            var json = $@"{{""clientId"": ""{fronteggCredentials.ClientId}"", ""secret"": ""{fronteggCredentials.ApiKey}""}}";

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