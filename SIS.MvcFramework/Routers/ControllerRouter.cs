namespace SIS.MvcFramework.Routers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework.ActionResults.Contracts;
    using SIS.MvcFramework.Attributes.Methods;
    using SIS.MvcFramework.Controllers;
    using SIS.WebServer.Api;
    using SIS.WebServer.Results;

    public class ControllerRouter : IHttpHandler
    {
        public IHttpResponse Handle(IHttpRequest request)
        {
            string controllerName = string.Empty;
            string actionName = string.Empty;
            string requestMethod = string.Empty;

            if (request.Url == "/")
            {
                controllerName = "Home";
                actionName = "Index";
                requestMethod = "GET";
            }
            else
            {
                string[] requestUrlSplit = request.Url.Split('/', StringSplitOptions.RemoveEmptyEntries);

                requestMethod = request.RequestMethod.ToString();

                controllerName = requestUrlSplit[0];
                actionName = requestUrlSplit[1];
            }

            Controller controller = this.GetController(controllerName, request);
            MethodInfo action = this.GetAction(requestMethod, controller, actionName);

            if (action == null)
            {
                return new HttpResponse(HttpStatusCode.NotFound);
            }

            return this.PrepareResponse(controller, action);
        }

        private IHttpResponse PrepareResponse(Controller controller, MethodInfo action)
        {
            IActionResult actionResult = action.Invoke(controller, null) as IActionResult;

            string invocationResult = actionResult.Invoke();

            if (actionResult is IViewable)
            {
                return new HtmleResult(invocationResult, HttpStatusCode.OK);
            }
            else if (actionResult is IRedirectable)
            {
                return new RedirectResult(invocationResult);
            }
            else
            {
                throw new InvalidOperationException("The view result is not supported.");
            }
        }

        private MethodInfo GetAction(string requestMethod, Controller controller, string actionName)
        {
            // TODO: check getsuitable method
            IEnumerable<MethodInfo> actions = this.GetSuitableMethos(controller, actionName);

            if (!actions.Any())
            {
                return null;
            }

            foreach (var action in actions)
            {
                IEnumerable<HttpMethodAttribute> httpMethodAttributes = action.GetCustomAttributes()
                    .Where(ca => ca is HttpMethodAttribute)
                    .Cast<HttpMethodAttribute>();

                if (!httpMethodAttributes.Any() && requestMethod.ToLower() == "get")
                {
                    return action;
                }

                foreach (var httpMethodAttribute in httpMethodAttributes)
                {
                    if (httpMethodAttribute.IsValid(requestMethod))
                    {
                        return action;
                    }
                }
            }

            return null;
        }

        private IEnumerable<MethodInfo> GetSuitableMethos(Controller controller, string actionName)
        {
            if (controller == null)
            {
                return new MethodInfo[0];
            }

            return controller
                .GetType()
                .GetMethods()
                .Where(mi => mi.Name.ToLower() == actionName.ToLower());
        }

        private Controller GetController(string controllerName, IHttpRequest request)
        {
            if (string.IsNullOrWhiteSpace(controllerName))
            {
                return null;
            }

            string fullyQualifiedControllerName = $"{MvcContext.Get.AssemblyName}.{MvcContext.Get.ControllersFolder}.{controllerName}{MvcContext.Get.ControllersSufix}, {MvcContext.Get.AssemblyName}";

            Type controllerType = Type.GetType(fullyQualifiedControllerName);

            Controller controller = Activator.CreateInstance(controllerType) as Controller;

            if (controller != null)
            {
                controller.Request = request;
            }

            return controller;
        }
    }
}
