namespace SIS.MvcFramework.ActionResults
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using SIS.MvcFramework.ActionResults.Contracts;

    public class ViewResult : IViewable
    {
        public ViewResult(IRendable view)
        {
            this.View = view;
        }

        public IRendable View { get; set; }

        public string Invoke() => this.View.Render();
    }
}
