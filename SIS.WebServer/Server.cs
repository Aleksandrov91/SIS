namespace SIS.WebServer
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using SIS.WebServer.Api;
    using SIS.WebServer.Routing;

    public class Server
    {
        private const string LocalHostIpAddress = "::1";

        private readonly int port;

        private readonly TcpListener tcpListener;

        private readonly IHttpHandler handler;

        private bool isRunning;

        public Server(int port, IHttpHandler handler)
        {
            this.port = port;
            this.tcpListener = new TcpListener(IPAddress.Parse(LocalHostIpAddress), this.port);

            this.handler = handler;
        }

        public void Run()
        {
            this.tcpListener.Start();
            this.isRunning = true;

            Console.WriteLine($"Server started at http://{LocalHostIpAddress}:{this.port}");

            Task task = Task.Run(this.ListenLoop);
            task.Wait();
        }

        public async Task ListenLoop()
        {
            while (this.isRunning)
            {
                Socket client = await this.tcpListener.AcceptSocketAsync();
                ConnectionHandler connectionHandler = new ConnectionHandler(client, this.handler);
                await connectionHandler.ProcessRequestAsync();
            }
        }
    }
}
