namespace SIS.WebServer
{
    using HTTP.Common;
    using HTTP.Cookies;
    using HTTP.Enums;
    using HTTP.Exceptions;
    using HTTP.Requests;
    using HTTP.Requests.Contracts;
    using HTTP.Responses;
    using HTTP.Responses.Contracts;
    using HTTP.Sessions;
    using Results;
    using Routing;
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    public class ConnectionHandler
    {
        private readonly Socket client;

        private readonly ServerRoutingTable serverRoutingTable;

        public ConnectionHandler(Socket client, ServerRoutingTable serverRoutingTable)
        {
            CoreValidator.ThrowIfNull(client, nameof(client));
            CoreValidator.ThrowIfNull(serverRoutingTable, nameof(serverRoutingTable));

            this.client = client;
            this.serverRoutingTable = serverRoutingTable;
        }

        private async Task<IHttpRequest> ReadRequest()
        {
            var result = new StringBuilder();
            var data = new ArraySegment<byte>(new byte[1024]);

            while (true)
            {

                int numberOfBytesRead = await this.client.ReceiveAsync(data.Array, SocketFlags.None);

                if (numberOfBytesRead == 0)
                {
                    break;
                }

                var bytesAsString = Encoding.UTF8.GetString(data.Array, 0, numberOfBytesRead);

                result.Append(bytesAsString);

                if (numberOfBytesRead < 1023)
                {
                    break;
                }
            }

            if (result.Length == 0)
            {
                return null;
            }

            return new HttpRequest(result.ToString());
        }

        private IHttpResponse HandleRequest(IHttpRequest httpRequest)
        {
            var isResourceRequest = this.IsResourceRequest(httpRequest.Path);

            if (isResourceRequest)
            {
                return this.HandleResourceResponce(httpRequest.Path);
            }

            if (!this.serverRoutingTable.Routes.ContainsKey(httpRequest.RequestMethod) || !this.serverRoutingTable.Routes[httpRequest.RequestMethod].ContainsKey(httpRequest.Path))
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            return this.serverRoutingTable.Routes[httpRequest.RequestMethod][httpRequest.Path].Invoke(httpRequest);
        }

        private IHttpResponse HandleResourceResponce(string httpPath)
        {
            var indexOfExtensionStart = httpPath.LastIndexOf('.');
            var indexOfNameStart = httpPath.LastIndexOf('/');
            var reqPathExtension = httpPath.Substring(indexOfExtensionStart);
            var resourceNameWithExt = httpPath.Substring(indexOfNameStart);
            //var executionAssembly = Assembly.GetExecutingAssembly().Location;
            var resourcePath = "../../../Resources/" + $"{reqPathExtension.Substring(1)}" + resourceNameWithExt ;

            if (!File.Exists(resourcePath))
            {
                 return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            var fileContent = File.ReadAllBytes(resourcePath);

            return new InlineResourceResult(fileContent,HttpResponseStatusCode.Ok);
        }

        private bool IsResourceRequest(string reqPath)
        {
            
            if (reqPath.Contains('.'))
            {
                var reqPathExtension = reqPath.Substring(reqPath.LastIndexOf('.'));
                var result = GlobalConstants.ResourceExtensions.Contains(reqPathExtension);
                return result;
            }

            return false;
        }

        private async Task PrepareResponse(IHttpResponse httpResponse)
        {
            byte[] byteSegments = httpResponse.GetBytes();

            await this.client.SendAsync(byteSegments, SocketFlags.None);

        }

        public async Task ProcessRequestAsync()
        {
            try
            {
                var httpRequest = await this.ReadRequest();

                if (httpRequest != null)
                {
                    string sessionId = this.SetRequestSession(httpRequest);

                    var httpResponse = this.HandleRequest(httpRequest);

                    this.SetResponseSession(httpResponse, sessionId);

                    await this.PrepareResponse(httpResponse);
                }


            }
            catch (BadRequestException e)
            {
                await this.PrepareResponse(new TextResult(e.ToString(), HttpResponseStatusCode.BadRequest));
            }
            catch (Exception e)
            {
                await this.PrepareResponse(new TextResult(e.ToString(), HttpResponseStatusCode.BadRequest));
            }

            this.client.Shutdown(SocketShutdown.Both);
        }

        private string SetRequestSession(IHttpRequest httpRequest)
        {
            string sessionId = null;

            if (httpRequest.Cookies.ContainsCookie(HttpSessionStorage.SessionCookieKey))
            {
                var cookie = httpRequest.Cookies.GetCookie(HttpSessionStorage.SessionCookieKey);
                sessionId = cookie.Value;
                httpRequest.Session = HttpSessionStorage.GetSession(sessionId);
            }
            else
            {
                sessionId = Guid.NewGuid().ToString();
                httpRequest.Session = HttpSessionStorage.GetSession(sessionId);
            }

            return sessionId;
        }

        private void SetResponseSession(IHttpResponse httpResponse, string sessionId)
        {
            if (sessionId != null)
            {
                httpResponse.AddCookie(new HttpCookie(HttpSessionStorage.SessionCookieKey, sessionId));
            }
        }
    }
}
