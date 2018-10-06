namespace IRunes.WebApp.Controllers
{
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;

    public class HomeController : BaseController
    {
        public IHttpResponse Index(IHttpRequest request)
        {
            if (this.IsAuthenticated(request))
            {
                this.ViewBag["username"] = request.Session.GetParameter("username").ToString();

                return this.View("LoggedIndex");
            }

            return this.View();
        }
    }
}
