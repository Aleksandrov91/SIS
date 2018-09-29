namespace SIS.WebServer.Results
{
    using System.Net;
    using System.Text;
    using SIS.HTTP.Headers;
    using SIS.HTTP.Responses;

    public class HtmleResult : HttpResponse
    {
        public HtmleResult(string content, HttpStatusCode statusCode)
            : base(statusCode)
        {
            this.Headers.Add(new HttpHeader("Content-type", "text/html"));
            this.Content = Encoding.UTF8.GetBytes(content);
        }
    }
}
