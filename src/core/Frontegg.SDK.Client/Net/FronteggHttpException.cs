using System;
using System.Net;
using System.Runtime.Serialization;

namespace Frontegg.SDK.Client.Net
{
    [Serializable]
    public class FronteggHttpException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        
        public FronteggHttpException(HttpStatusCode statusCode):
            this(statusCode, "An exception has occured while executing http message to Frontegg.")
        {
        }

        public FronteggHttpException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public FronteggHttpException(HttpStatusCode statusCode, string message, Exception inner) 
            : base(message, inner)
        {
            StatusCode = statusCode;
        }

        protected FronteggHttpException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}