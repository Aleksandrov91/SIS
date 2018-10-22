namespace SIS.MvcFramework.Attributes.Methods
{
    public class HttpDeleteAttribute : HttpMethodAttribute
    {
        public override bool IsValid(string requestMethod) => requestMethod.ToUpper() == "DELETE";
    }
}
