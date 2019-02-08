namespace IRunes.WebApp
{
    using IRunes.WebApp.Data;
    using IRunes.WebApp.Services;
    using IRunes.WebApp.Services.Contracts;
    using Microsoft.EntityFrameworkCore;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Routers;
    using SIS.MvcFramework.Services;
    using SIS.MvcFramework.Services.Contracts;
    using SIS.WebServer;
    using SIS.WebServer.Api;

    public class Launcher
    {
        public static void Main(string[] args)
        {
            IDependencyContainer dependencyContainer = new DependencyContainer();
            IHttpHandler handler = new ControllerRouter(dependencyContainer);
            IHttpHandler resourceHandler = new ResourceRouter();

            dependencyContainer.RegisterDependency<IHashService, HashService>();

            Server server = new Server(80, handler, resourceHandler);
            MvcEngine.Run(server);
        }
    }
}
