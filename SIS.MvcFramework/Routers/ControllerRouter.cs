namespace SIS.MvcFramework.Routers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
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

            if (request.Path == "/")
            {
                controllerName = "Home";
                actionName = "Index";
                requestMethod = "GET";
            }
            else
            {
                string[] requestUrlSplit = request.Path.Split('/', StringSplitOptions.RemoveEmptyEntries);

                if (requestUrlSplit.Length < 2)
                {
                    return new HttpResponse(HttpStatusCode.NotFound);
                }

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

            object[] actionParameters = this.MapActionParameters(controller, action, request);

            IActionResult actionResult = this.InvokeAction(controller, action, actionParameters);

            return this.PrepareResponse(actionResult);
        }

        private IHttpResponse PrepareResponse(IActionResult actionResult)
        {
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

        private object[] MapActionParameters(Controller controller, MethodInfo action, IHttpRequest request)
        {
            ParameterInfo[] actionParametersInfo = action.GetParameters();
            object[] mappedActionParameters = new object[actionParametersInfo.Length];

            for (var i = 0; i < actionParametersInfo.Length; i++)
            {
                var currentParameter = actionParametersInfo[i];

                if (currentParameter.ParameterType.IsPrimitive ||
                    currentParameter.ParameterType == typeof(string))
                {
                    mappedActionParameters[i] = this.ProccessPrimitiveParameter(currentParameter, request);
                }
                else
                {
                    object bindingModel = this.ProccessBindingModelParameter(currentParameter, request);
                    controller.ModelState.IsValid = this.IsValidModel(bindingModel, currentParameter.ParameterType);
                    mappedActionParameters[i] = bindingModel;
                }
            }

            return mappedActionParameters;
        }

        private bool IsValidModel(object bindingModel, Type bindingModelType)
        {
            PropertyInfo[] modelProperties = bindingModelType.GetProperties();

            foreach (var prop in modelProperties)
            {
                var propertyValidationAttributes = prop.GetCustomAttributes<ValidationAttribute>();

                foreach (var validationAttribute in propertyValidationAttributes)
                {
                    var propertyValue = prop.GetValue(bindingModel);

                    if (!validationAttribute.IsValid(propertyValue))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private object ProccessBindingModelParameter(ParameterInfo param, IHttpRequest request)
        {
            Type bindingModelType = param.ParameterType;

            object bindingModelInstance = Activator.CreateInstance(bindingModelType);
            IEnumerable<PropertyInfo> bindingModelProperties = bindingModelType.GetRuntimeProperties();

            foreach (var property in bindingModelProperties)
            {
                try
                {
                    object value = this.GetParameterFromRequestData(request, property.Name.ToLower());
                    property.SetValue(bindingModelInstance, Convert.ChangeType(value, property.PropertyType));
                }
                catch
                {
                    Console.WriteLine($"The {property.Name} field could not be mapped.");
                }
            }

            return Convert.ChangeType(bindingModelInstance, bindingModelType);
        }

        private object ProccessPrimitiveParameter(ParameterInfo actionParameter, IHttpRequest request)
        {
            object value = this.GetParameterFromRequestData(request, actionParameter.Name.ToLower());

            return Convert.ChangeType(value, actionParameter.ParameterType);
        }

        private object GetParameterFromRequestData(IHttpRequest request, string paramName)
        {
            // TODO: To lower
            if (request.QueryData.ContainsKey(paramName.ToLower()))
            {
                return request.QueryData[paramName.ToLower()];
            }

            if (request.FormData.ContainsKey(paramName.ToLower()))
            {
                return request.FormData[paramName.ToLower()];
            }

            return null;
        }

        private IActionResult InvokeAction(Controller controller, MethodInfo action, object[] actionParameters)
        {
            return action.Invoke(controller, actionParameters) as IActionResult;
        }
    }
}
