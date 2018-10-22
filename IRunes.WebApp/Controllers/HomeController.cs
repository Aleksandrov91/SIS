namespace IRunes.WebApp.Controllers
{
    using SIS.HTTP.Requests.Contracts;
    using SIS.MvcFramework.ActionResults.Contracts;
    using SIS.MvcFramework.Attributes.Methods;

    public class HomeController : BaseController
    {
        [HttpGet]
        public IActionResult Index()
        {
            //if (this.IsAuthenticated(request))
            //{
            //    this.ViewBag["username"] = request.Session.GetParameter("username").ToString();

            //    return this.View("LoggedIndex");
            //}

            return this.View();
        }
    }
}
