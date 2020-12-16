using System;
using System.Runtime.Serialization;

namespace Frontegg.SDK.AspNet.Owin.Authentication
{
    [Serializable]
    public class FailedAuthorisationsException : Exception
    {
        public FailedAuthorisationsException(string message) 
            : base(message)
        {
        }

        public FailedAuthorisationsException(string message, Exception inner) : base(message, inner)
        {
        }

        protected FailedAuthorisationsException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}