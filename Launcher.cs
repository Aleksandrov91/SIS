using SIS.HTTP.Enums;
using SIS.WebServer;
using SIS.WebServer.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Demo
{
    class Launcher
    {
        static void Main(string[] args)
        {
            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/"] = request => new HomeController().Index();

            Server server = new Server(80, serverRoutingTable);
            server.Run();
        }
    }
}
