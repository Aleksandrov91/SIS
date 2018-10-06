namespace SIS.WebServer.Results
{
    using SIS.HTTP.Responses;

    public class BadRequest : HttpResponse
    {
        // TODO
        public BadRequest()
            :base(System.Net.HttpStatusCode.BadRequest)
        {

        }
    }
}
