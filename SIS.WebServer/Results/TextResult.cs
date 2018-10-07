namespace SIS.WebServer.Results
{
    using System.Net;
    using System.Text;
    using SIS.HTTP.Headers;
    using SIS.HTTP.Responses;

    public class TextResult : HttpResponse
    {
        public TextResult(string content, HttpStatusCode statusCode)
            : base(statusCode)
        {
            this.Headers.Add(new HttpHeader("Content-type", "text/plain; charset=utf-8"));
            this.Content = Encoding.UTF8.GetBytes(content);
        }
    }
}
