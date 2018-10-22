namespace SIS.MvcFramework.Controllers
{
    using System.Runtime.CompilerServices;
    using SIS.HTTP.Requests.Contracts;
    using SIS.MvcFramework.ActionResults;
    using SIS.MvcFramework.ActionResults.Contracts;
    using SIS.MvcFramework.Utilities;
    using SIS.MvcFramework.Views;

    public abstract class Controller
    {
        protected Controller()
        {
        }

        public IHttpRequest Request { get; set; }

        protected IViewable View([CallerMemberName] string viewName = "")
        {
            string controllerName = ControllerUtilities.GetControllerName(this);

            string viewFullyQualifiedName = ControllerUtilities.GetFullyQualifiedName(controllerName, viewName);

            IRendable view = new View(viewFullyQualifiedName);

            return new ViewResult(view);
        }

        protected IRedirectable RedirectToAction(string redirectUrl) => new RedirectResult(redirectUrl);
    }
}
