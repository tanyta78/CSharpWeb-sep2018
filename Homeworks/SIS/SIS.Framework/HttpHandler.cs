namespace SIS.Framework
{
    using HTTP.Common;
    using HTTP.Requests.Contracts;
    using HTTP.Responses.Contracts;
    using System.Linq;
    using Routers;
    using WebServer.Api;

    public class HttpHandler : IHttpHandler
    {
       public IHttpResponse Handle(IHttpRequest request)
        {
            var isResourceRequest = this.IsResourceRequest(request.Path);

            if (isResourceRequest)
            {
                return new ResourceRouter().Handle(request);
            }

            return new ControllerRouter().Handle(request);
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