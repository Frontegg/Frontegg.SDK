using System;
using System.Runtime.Serialization;

namespace Frontegg.SDK.Core.Authentication
{
    [Serializable]
    public class MissingCredentialsException : Exception
    {
        public MissingCredentialsException()
        {
        }

        public MissingCredentialsException(string message) : base(message)
        {
        }

        public MissingCredentialsException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MissingCredentialsException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}