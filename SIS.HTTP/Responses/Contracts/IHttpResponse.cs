namespace SIS.HTTP.Responses.Contracts
{
    using System.Net;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Cookies.Contracts;
    using SIS.HTTP.Headers;
    using SIS.HTTP.Headers.Contracts;

    public interface IHttpResponse
    {
        HttpStatusCode StatusCode { get; set; }

        IHttpHeaderCollection Headers { get; }

        IHttpCookieCollection Cookies { get; }

        byte[] Content { get; }

        void AddHeader(HttpHeader header);

        void AddCookie(HttpCookie cookie);

        byte[] GetBytes();
    }
}
