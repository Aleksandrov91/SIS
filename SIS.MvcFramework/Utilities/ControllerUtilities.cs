namespace SIS.MvcFramework.Utilities
{
    using SIS.HTTP.Common;

    public static class ControllerUtilities
    {
        public static string GetControllerName(object controller) =>
            controller.GetType()
            .Name
            .Replace(MvcContext.Get.ControllersSufix, string.Empty);

        public static string GetFullyQualifiedName(string controllerName, string viewName) =>
            $"{MvcContext.Get.ViewsFolder}/{controllerName}/{viewName}{GlobalConstants.HtmlFileExtension}";
    }
}
