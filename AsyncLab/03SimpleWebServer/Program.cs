namespace _03SimpleWebServer
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    public class Program
    {
        public static void Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 1337;

            TcpListener listener = new TcpListener(ipAddress, port);
            listener.Start();

            Console.WriteLine($"Server started listening to TCP clients at 127.0.0.1:{port}");

            Task.Run(async () => { await ConnectWithTcpClient(listener); }).GetAwaiter().GetResult();
        }

        private static async Task ConnectWithTcpClient(TcpListener listener)
        {
            while (true)
            {
                //connecting
                Console.WriteLine("Waiting for client ...");
                var client = await listener.AcceptTcpClientAsync();

                Console.WriteLine("Client connected.");

                //read request
                byte[] buffer = new byte[1024];
                await client.GetStream().ReadAsync(buffer, 0, buffer.Length);

                var clientMessage = Encoding.UTF8.GetString(buffer);
                Console.WriteLine(clientMessage.Trim('\0'));


                //sending
                byte[] responseMessage = Encoding.UTF8.GetBytes("Hello from my server! А това е на български!!!");
                await client.GetStream().WriteAsync(responseMessage, 0, responseMessage.Length);

                Console.WriteLine("Closing connection.");
                client.Dispose();
            }
        }
    }
}
