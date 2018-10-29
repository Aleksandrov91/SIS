namespace IRunes.WebApp.Controllers
{
    using IRunes.WebApp.ViewModels;
    using SIS.HTTP.Requests.Contracts;
    using SIS.MvcFramework.ActionResults.Contracts;
    using SIS.MvcFramework.Attributes.Methods;

    public class HomeController : BaseController
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (this.IsAuthenticated(this.Request))
            {
                this.ViewBag["username"] = this.Request.Session.GetParameter("username").ToString();

                return this.View("LoggedIndex");
            }

            this.Model.Data["username"] = "Sasho";

            return this.View();
        }
    }
}
