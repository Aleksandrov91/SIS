namespace SIS.WebServer.Results
{
    using System.Net;
    using SIS.HTTP.Headers;
    using SIS.HTTP.Responses;

    public class RedirectResult : HttpResponse
    {
        public RedirectResult(string location)
            : base(HttpStatusCode.SeeOther)
        {
            this.Headers.Add(new HttpHeader("Location", location));
        }
    }
}
