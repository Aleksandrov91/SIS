﻿namespace Cakes.WebApp.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Runtime.CompilerServices;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;

    public abstract class BaseController
    {
        protected BaseController()
        {
            //this.IRunesContext = new IRunesContext();
            this.ViewBag = new Dictionary<string, string>();
        }

        protected static IDictionary<string, string> TempData { get; private set; } = new Dictionary<string, string>
        {
            ["errorMessage"] = string.Empty
        };

        //protected IRunesContext IRunesContext { get; private set; }

        protected IDictionary<string, string> ViewBag { get; private set; }

        protected IHttpResponse View([CallerMemberName] string viewName = "")
        {
            string filePath = $"{GlobalConstants.ViewsFolderName}/{this.GetCurrentControllerName()}/{viewName}{GlobalConstants.HtmlFileExtension}";

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"View {viewName} not found!");
            }

            string content = File.ReadAllText(filePath);

            foreach (var viewBagKey in this.ViewBag.Keys)
            {
                if (content.Contains($"{{{{{viewBagKey}}}}}"))
                {
                    content = content.Replace($"{{{{{viewBagKey}}}}}", this.ViewBag[viewBagKey]);
                }
            }

            foreach (var tempDataKey in TempData.Keys)
            {
                if (content.Contains($"{{{{{tempDataKey}}}}}"))
                {
                    content = content.Replace($"{{{{{tempDataKey}}}}}", TempData[tempDataKey]);
                }
            }

            TempData.Clear();
            TempData["errorMessage"] = string.Empty;

            return new HtmleResult(content, HttpStatusCode.OK);
        }

        protected IHttpResponse RedirectToAction(string route) => new RedirectResult(route);

        protected IHttpResponse BadRequestError(string message)
        {
            string content = $"<h1>{message}</h1>";

            return new HtmleResult(content, HttpStatusCode.BadRequest);
        }

        protected bool IsAuthenticated(IHttpRequest request) => request.Session.ContainsParameter("username");

        private string GetCurrentControllerName() => this.GetType().Name.Split("Controller").FirstOrDefault();
    }
}
