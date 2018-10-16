namespace SIS.Framework
{
    using System.IO;
    using System.Linq;
    using HTTP.Common;
    using HTTP.Enums;
    using HTTP.Requests.Contracts;
    using HTTP.Responses;
    using HTTP.Responses.Contracts;
    using WebServer.Api;
    using WebServer.Results;
    using WebServer.Routing;

    public class HttpHandlerOld : IHttpHandler
    {
        private ServerRoutingTable serverRoutingTable;

        public HttpHandlerOld(ServerRoutingTable routingTable)
        {
            this.serverRoutingTable = routingTable;
        }

        public IHttpResponse Handle(IHttpRequest request)
        {
            var isResourceRequest = this.IsResourceRequest(request.Path);

            if (isResourceRequest)
            {
                return this.HandleResourceResponce(request.Path);
            }

            if (!this.serverRoutingTable.Routes.ContainsKey(request.RequestMethod) || !this.serverRoutingTable.Routes[request.RequestMethod].ContainsKey(request.Path))
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            return this.serverRoutingTable.Routes[request.RequestMethod][request.Path].Invoke(request);
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
    }
}