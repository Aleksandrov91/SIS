namespace SIS.MvcFramework
{
    using System.IO;
    using System.Net;
    using System.Text;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Api;
    using SIS.WebServer.Results;
    using SIS.WebServer.Routing;

    public class HttpHandler : IHttpHandler
    {
        private ServerRoutingTable routingTable;

        public HttpHandler(ServerRoutingTable routingTable)
        {
            this.routingTable = routingTable;
        }

        public IHttpResponse Handle(IHttpRequest request)
        {
            if (!this.routingTable.Routes.ContainsKey(request.RequestMethod) ||
            !this.routingTable.Routes[request.RequestMethod].ContainsKey(request.Path))
            {
                return this.ReturnIfResource(request.Path);
            }

            return this.routingTable.Routes[request.RequestMethod][request.Path].Invoke(request);
        }

        private IHttpResponse ReturnIfResource(string path)
        {
            string resourceFilePath = $"../../..{path}";

            if (File.Exists(resourceFilePath))
            {
                string fileContent = File.ReadAllText(resourceFilePath);
                byte[] byteContent = Encoding.UTF8.GetBytes(fileContent);

                return new InlineResourceResult(byteContent, HttpStatusCode.OK);
            }

            return new HttpResponse(HttpStatusCode.NotFound);
        }
    }
}
