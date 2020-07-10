using System;
using System.IO;

namespace Frontegg.SDK.Client.Authentication
{
    internal class FronteggAuthenticationResult
    { 
        public string Token { get; private set; }
        public int ExpiredIn { get; private set; }
        public bool IsAuthenticated { get; private set; }
        public string FailureReason { get; set; }
        
        public FronteggAuthenticationResult(string token, int expiredIn)
        {
            Token = !string.IsNullOrWhiteSpace(token) ? token : throw new ArgumentNullException(nameof(token));
            ExpiredIn = expiredIn > 0
                ? expiredIn
                : throw new InvalidDataException("Authentication expiry is lower than 0.");
            IsAuthenticated = true;
        }

        private FronteggAuthenticationResult(string failureReason)
        {
            FailureReason = failureReason;
            IsAuthenticated = false;
        }

        private FronteggAuthenticationResult()
        {
            
        }
        
        public static FronteggAuthenticationResult FailedResult(string failureReason)
        {
            if (string.IsNullOrWhiteSpace(failureReason))
            {
                failureReason = string.Empty;
            }
            
            return new FronteggAuthenticationResult(failureReason);
        }

        internal static FronteggAuthenticationResult CloneResult(FronteggAuthenticationResult source)
        {
            return new FronteggAuthenticationResult()
            {
                Token = source.Token,
                ExpiredIn = source.ExpiredIn,
                FailureReason = source.FailureReason,
                IsAuthenticated = source.IsAuthenticated
            };
        }
    }
}