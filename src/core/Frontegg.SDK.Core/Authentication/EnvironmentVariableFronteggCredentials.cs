using System;

namespace Frontegg.SDK.Core.Authentication
{
    public class EnvironmentVariableFronteggCredentials : IFronteggCredentials
    {
        private const string ClientIdVariableName = "FRONTEGG_CLIENT_ID";
        private const string ApiKeyVariableName = "FRONT_API_KEY";
        
        public string ClientId { get; }
        public string ApiKey { get; }

        public EnvironmentVariableFronteggCredentials()
        {
            ClientId = Environment.GetEnvironmentVariable(ClientIdVariableName) ?? throw new MissingEnvironmentVariableCredentialsException(ClientIdVariableName);
            ApiKey = Environment.GetEnvironmentVariable(ApiKeyVariableName) ?? throw new MissingEnvironmentVariableCredentialsException(ApiKeyVariableName);
        }
    }
}