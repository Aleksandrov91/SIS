namespace SIS.MvcFramework.Views
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using SIS.MvcFramework.ActionResults.Contracts;

    public class View : IRendable
    {
        private readonly string fullyQualifiedTemplateName;

        private readonly IDictionary<string, object> viewData;

        public View(string fullyQualifiedTemplateName, IDictionary<string, object> viewData)
        {
            this.fullyQualifiedTemplateName = fullyQualifiedTemplateName;
            this.viewData = viewData;
        }

        public string Render()
        {
            string fullHtml = this.ReadFile();

            return this.RenderHtml(fullHtml);
        }

        private string RenderHtml(string fullHtml)
        {
            string renderedHtml = fullHtml;

            if (this.viewData.Any())
            {
                foreach (var parameter in this.viewData)
                {
                    renderedHtml = renderedHtml.Replace($"{{{{{{{parameter.Key}}}}}}}", parameter.Value.ToString());
                }
            }

            return renderedHtml;
        }

        private string ReadFile()
        {
            if (!File.Exists(this.fullyQualifiedTemplateName))
            {
                throw new FileNotFoundException($"View does not exists at {this.fullyQualifiedTemplateName}");
            }

            return File.ReadAllText(this.fullyQualifiedTemplateName);
        }
    }
}
