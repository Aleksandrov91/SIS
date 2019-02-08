namespace SIS.MvcFramework.Controllers
{
    using System.Runtime.CompilerServices;
    using SIS.HTTP.Requests.Contracts;
    using SIS.MvcFramework.ActionResults;
    using SIS.MvcFramework.ActionResults.Contracts;
    using SIS.MvcFramework.Models;
    using SIS.MvcFramework.Utilities;
    using SIS.MvcFramework.Views;

    public abstract class Controller
    {
        protected Controller()
        {
            this.Model = new ViewModel();
            this.ModelState = new Model();
        }

        public Model ModelState { get; set; }

        public ViewModel Model { get; }

        public IHttpRequest Request { get; set; }

        protected IViewable View([CallerMemberName] string viewName = "")
        {
            string controllerName = ControllerUtilities.GetControllerName(this);

            string viewFullyQualifiedName = ControllerUtilities.GetFullyQualifiedName(controllerName, viewName);

            IRendable view = new View(viewFullyQualifiedName, this.Model.Data);

            return new ViewResult(view);
        }

        protected IRedirectable RedirectToAction(string redirectUrl) => new RedirectResult(redirectUrl);
    }
}
