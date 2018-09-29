namespace SIS.HTTP.Responses.Contracts
{
    using System.Net;
    using SIS.HTTP.Headers;
    using SIS.HTTP.Headers.Contracts;

    public interface IHttpResponse
    {
        HttpStatusCode StatusCode { get; set; }

        IHttpHeaderCollection Headers { get; }

        byte[] Content { get; }

        void AddHeader(HttpHeader header);

        byte[] GetBytes();
    }
}
