namespace SIS.HTTP.Responses
{
    using System.Linq;
    using System.Net;
    using System.Text;
    using SIS.HTTP.Common;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Cookies.Contracts;
    using SIS.HTTP.Exceptions;
    using SIS.HTTP.Extensions;
    using SIS.HTTP.Headers;
    using SIS.HTTP.Headers.Contracts;
    using SIS.HTTP.Responses.Contracts;

    public class HttpResponse : IHttpResponse
    {
        public HttpResponse()
        {
        }

        public HttpResponse(HttpStatusCode statusCode)
        {
            this.Headers = new HttpHeaderCollection();
            this.Content = new byte[0];
            this.StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; set; }

        public IHttpHeaderCollection Headers { get; private set; }

        public IHttpCookieCollection Cookies { get; private set; }

        public byte[] Content { get; set; }

        public void AddHeader(HttpHeader header)
        {
            if (string.IsNullOrEmpty(header.Key) || string.IsNullOrEmpty(header.Value))
            {
                throw new BadRequestException();
            }

            this.Headers.Add(header);
        }

        public void AddCookie(HttpCookie cookie)
        {
            this.Cookies.Add(cookie);
        }

        public byte[] GetBytes()
        {
            return Encoding.UTF8.GetBytes(this.ToString()).Concat(this.Content).ToArray();
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine($"{GlobalConstants.HttpOneProtocolFragment} {this.StatusCode.GetResponseLine()}");
            result.AppendLine(this.Headers.ToString());

            if (this.Cookies.HasCookies())
            {
                result.AppendLine($"Set-Cookie: {this.Cookies}");
            }

            result.AppendLine();

            return result.ToString();
        }
    }
}
