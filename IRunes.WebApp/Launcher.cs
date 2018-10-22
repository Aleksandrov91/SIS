namespace IRunes.WebApp
{
    using SIS.MvcFramework;
    using SIS.MvcFramework.Routers;
    using SIS.WebServer;
    using SIS.WebServer.Api;

    public class Launcher
    {
        public static void Main(string[] args)
        {
            IHttpHandler handler = new ControllerRouter();

            Server server = new Server(80, handler);
            MvcEngine.Run(server);
        }
    }
}
