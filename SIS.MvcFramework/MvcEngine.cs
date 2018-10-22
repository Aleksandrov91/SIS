namespace SIS.MvcFramework
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using SIS.WebServer;

    public static class MvcEngine
    {
        public static void Run(Server webServer)
        {
            //MvcContext.Get.AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            MvcContext.Get.AssemblyName = Assembly.GetEntryAssembly()
                .GetName()
                .Name;

            try
            {
                webServer.Run();
            }
            catch (Exception e)
            {
            }
        }
    }
}
