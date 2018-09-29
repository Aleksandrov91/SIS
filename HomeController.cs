using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Results;
using System;
using System.Net;

namespace SIS.Demo
{
    public class HomeController
    {
        public IHttpResponse Index()
        {
            string content = "<h1>Hello Word</h1>";

            return new HtmleResult(content, HttpStatusCode.OK);
        }
    }
}
