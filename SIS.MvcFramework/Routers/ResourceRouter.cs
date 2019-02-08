namespace SIS.MvcFramework.Routers
{
    using System.IO;
    using System.Net;
    using System.Text;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Api;
    using SIS.WebServer.Results;

    public class ResourceRouter : IHttpHandler
    {
        public IHttpResponse Handle(IHttpRequest request)
        {
            string path = request.Path;

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
