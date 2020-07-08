using System.Net;
using System.Net.Http;

namespace Frontegg.SDK.AspNet.Http
{
    public class ForbiddenHttpResponseMessage: HttpResponseMessage
    {
        private const string Response = @"{""statusCode"":401,""error"":""Unauthorized"",""message"":""Could not verify vendor""}";
        
        public ForbiddenHttpResponseMessage()
            : base(HttpStatusCode.Forbidden)
        {
            Content = new StringContent(Response);
        }
    }
}