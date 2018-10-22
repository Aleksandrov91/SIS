namespace SIS.MvcFramework.Views
{
    using System.IO;
    using SIS.MvcFramework.ActionResults.Contracts;

    public class View : IRendable
    {
        private readonly string fullyQualifiedTemplateName;

        public View(string fullyQualifiedTemplateName)
        {
            this.fullyQualifiedTemplateName = fullyQualifiedTemplateName;
        }

        public string Render() => this.ReadFile();

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
