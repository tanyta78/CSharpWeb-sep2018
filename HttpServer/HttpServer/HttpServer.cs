namespace HttpServer
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
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

    public class SessionManager
    {
        private readonly IDictionary<string, int> sessionData = new ConcurrentDictionary<string, int>();

        public void CreateSession(string sessionId)
        {
            this.sessionData.Add(sessionId, 0);
        }

        public int GetSession(string sessionId)
        {
            this.sessionData.TryGetValue(sessionId, out int value);
            return value;
        }

        public void SetSessionData(string sessionId, int newValue)
        {
            this.sessionData[sessionId] = newValue;
        }

        public bool Exist(string sessionId)
        {
            return this.sessionData.TryGetValue(sessionId, out _);
        }
    }

    public class RequestProcessor
    {
        SessionManager sessionManager = new SessionManager();

        object lockObj = new object();

        public async Task ProcessClient(TcpClient client)
        {
            var buffer = new byte[10240];
            var stream = client.GetStream();

            //Console.WriteLine($"{client.Client.RemoteEndPoint} {Thread.CurrentThread.ManagedThreadId}");

            var readLength = await stream.ReadAsync(buffer, 0, buffer.Length);
            var requestText = Encoding.UTF8.GetString(buffer, 0, readLength);


            var sessionId = ParseSessionId(requestText);

            Console.WriteLine(new string('=', 60));
            Console.WriteLine(requestText);
            //var responseText = File.ReadAllText("index.html") + "Hello, " + sessionId;

            string sessionSetCookie = null;
            if (!this.sessionManager.Exist(sessionId))
            {
                var newSessionId = Guid.NewGuid().ToString();
                this.sessionManager.CreateSession(newSessionId);
                sessionSetCookie = "Set-Cookie: SessionId=" + newSessionId + "; Max-Age=8000000" + Environment.NewLine;
            }
            else
            {
                lock (this.lockObj)
                {
                    var data = this.sessionManager.GetSession(sessionId);
                    data++;
                    this.sessionManager.SetSessionData(sessionId, data);
                }

            }

            var sessionData = this.sessionManager.GetSession(sessionId);
            var responseText = "Hello, " + sessionId + " " + sessionData;

            var responseBytes = Encoding.UTF8.GetBytes(
                "HTTP/1.0 200 OK" + Environment.NewLine +
                "Content-Type: text/html" + Environment.NewLine +
                 sessionSetCookie +
                "Content-Length: " + responseText.Length + Environment.NewLine + Environment.NewLine +
                responseText);
            await stream.WriteAsync(responseBytes);
            //Console.WriteLine($"{client.Client.RemoteEndPoint} => {Thread.CurrentThread.ManagedThreadId}");
        }

        private static string ParseSessionId(string request)
        {
            var sr = new StringReader(request);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.StartsWith("Cookie: "))
                {
                    var lineParts = line.Split(": ", StringSplitOptions.RemoveEmptyEntries);
                    if (lineParts.Length == 2)
                    {
                        var cookies = lineParts[1].Split("; ", StringSplitOptions.RemoveEmptyEntries);
                        foreach (var cookie in cookies)
                        {
                            var cookieParts = cookie.Split("=", 2, StringSplitOptions.RemoveEmptyEntries);
                            if (cookieParts.Length == 2)
                            {
                                var cookieName = cookieParts[0];
                                var cookieValue = cookieParts[1];
                                if (cookieName == "SessionId")
                                {
                                    return cookieValue;
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}