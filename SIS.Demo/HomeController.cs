namespace SIS.Demo
{
    using System.Net;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;

    public class HomeController
    {
        public IHttpResponse Index()
        {
            string content = "<h1>Hello World!</h1>";

            return new HtmleResult(content, HttpStatusCode.OK);
        }
    }
}
