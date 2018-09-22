namespace HttpServer
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    public static class Program
    {
        public static void Main(string[] args)
        {
            IHttpServer server = new HttpServer();

            server.Start();
        }
    }

    public interface IHttpServer
    {
        void Start();

        void Stop();
    }

    public class HttpServer : IHttpServer
    {
        private TcpListener tcpListener;

        private Boolean isWorking;

        public HttpServer()
        {
            this.tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 80);
        }

        public void Start()
        {
            this.isWorking = true;
            this.tcpListener.Start();

            Console.WriteLine("Server is listening on port 80");

            while (this.isWorking)
            {
                var client = this.tcpListener.AcceptTcpClient();
                var buffer = new byte[10240];
                var stream = client.GetStream();
                var readLength = stream.Read(buffer, 0, buffer.Length);
                var requestText = Encoding.UTF8.GetString(buffer, 0, readLength);
                Console.WriteLine(new string('=', 60));
                Console.WriteLine(requestText);
                var responseText = File.ReadAllText("index.html");
                var responseBytes = Encoding.UTF8.GetBytes(
                    "HTTP/1.0 200 OK" + Environment.NewLine +
                    //// "Content-Type: text/plain" + Environment.NewLine +
                    //// "Location: https://softuni.bg" + Environment.NewLine + Environment.NewLine
                    //// "Content-Type: text/html" + Environment.NewLine +
                    "Content-Disposition: attachment; filename=index.html" + Environment.NewLine +
                    //// "Content-Type: text/plain" + Environment.NewLine +
                    "Content-Length: " + responseText.Length + Environment.NewLine + Environment.NewLine +
                    responseText);
                stream.Write(responseBytes);
            }
        }

        public void Stop()
        {
            this.isWorking = false;
        }
    }
}
