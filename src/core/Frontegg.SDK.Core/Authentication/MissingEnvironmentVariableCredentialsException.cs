using System;
using System.Runtime.Serialization;

namespace Frontegg.SDK.Core.Authentication
{
    [Serializable]
    public class MissingEnvironmentVariableCredentialsException : MissingCredentialsException
    {
        public MissingEnvironmentVariableCredentialsException(string variableName) 
            : base(GetMessage(variableName))
        {
        }

        public MissingEnvironmentVariableCredentialsException(string variableName, Exception inner) 
            : base(GetMessage(variableName), inner)
        {
        }

        protected MissingEnvironmentVariableCredentialsException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        private static string GetMessage(string variableName)
        {
            return $"Unable to find credentials {variableName} environment variable";
        }
    }
}