namespace SIS.WebServer
{
    using Api;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    public class Server
    {
        private const string LocalhostIpAddress = "127.0.0.1";

        private readonly int port;

        private readonly TcpListener listener;

        private readonly IHttpHandler handler;

        private bool isRunning;

        //public Server(int port, ServerRoutingTable serverRoutingTable)
        //{
        //    this.port = port;
        //    this.listener = new TcpListener(IPAddress.Parse(LocalhostIpAddress), port);
        //    this.handler = new HttpHandler(serverRoutingTable);
        //}

        public Server(int port, IHttpHandler handler)
        {
            this.port = port;
            this.listener = new TcpListener(IPAddress.Parse(LocalhostIpAddress), port);
            this.handler = handler;
        }

        public void Run()
        {
            this.listener.Start();
            this.isRunning = true;

            Console.WriteLine($"Server started at http://{LocalhostIpAddress}:{this.port}");

            while (this.isRunning)
            {
                Console.WriteLine("Waiting for client...");

                var client = listener.AcceptSocketAsync().GetAwaiter().GetResult();

                Task.Run(() => Listen(client));
            }
        }

        public async void Listen(Socket client)
        {
            var connectionHandler = new ConnectionHandler(client, this.handler);
            await connectionHandler.ProcessRequestAsync();
        }


    }
}
