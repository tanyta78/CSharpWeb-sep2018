namespace HttpServer
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class HttpServer : IHttpServer
    {
        private readonly TcpListener tcpListener;

        private Boolean isWorking;
        private readonly RequestProcessor reqProcessor;

        public HttpServer()
        {
            this.tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 80);
            this.reqProcessor = new RequestProcessor();
        }

        public void Start()
        {
            this.isWorking = true;
            this.tcpListener.Start();

            Console.WriteLine("Server is listening on port 80");

            while (this.isWorking)
            {
                var client = this.tcpListener.AcceptTcpClient();
#pragma warning disable 4014
                this.reqProcessor.ProcessClient(client);
#pragma warning restore 4014
            }
        }

   

        public void Stop()
        {
            this.isWorking = false;
        }
    }

    public class RequestProcessor
    {
        public async Task ProcessClient(TcpClient client)
        {
            var buffer = new byte[10240];
            var stream = client.GetStream();

            //Console.WriteLine($"{client.Client.RemoteEndPoint} {Thread.CurrentThread.ManagedThreadId}");

            var readLength = await stream.ReadAsync(buffer, 0, buffer.Length);
            var requestText = Encoding.UTF8.GetString(buffer, 0, readLength);

            Console.WriteLine(new string('=', 60));
            Console.WriteLine(requestText);

            var responseText = File.ReadAllText("index.html");
            var responseBytes = Encoding.UTF8.GetBytes(
                "HTTP/1.0 200 OK" + Environment.NewLine +
                "Content-Type: text/html" + Environment.NewLine +
                "Set-Cookie: lang=bg" + Environment.NewLine +
                "Content-Length: " + responseText.Length + Environment.NewLine + Environment.NewLine +
                responseText);
            await stream.WriteAsync(responseBytes);
            //Console.WriteLine($"{client.Client.RemoteEndPoint} => {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}