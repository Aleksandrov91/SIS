namespace SIS.MvcFramework.ActionResults.Contracts
{
    public interface IViewable : IActionResult
    {
        IRendable View { get; set; } 
    }
}
