namespace SIS.Framework.Routers
{
    using HTTP.Enums;
    using HTTP.Responses;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using System.IO;
    using WebServer.Api;
    using WebServer.Results;

    public class ResourceRouter : IHttpHandler
    {
        public IHttpResponse Handle(IHttpRequest request)
        {
            var httpPath = request.Path;

            var indexOfExtensionStart = httpPath.LastIndexOf('.');
            var indexOfNameStart = httpPath.LastIndexOf('/');
            var reqPathExtension = httpPath.Substring(indexOfExtensionStart);
            var resourceNameWithExt = httpPath.Substring(indexOfNameStart);
            //var executionAssembly = Assembly.GetExecutingAssembly().Location;
            var resourcePath = "../../../Resources/" + $"{reqPathExtension.Substring(1)}" + resourceNameWithExt;

            if (!File.Exists(resourcePath))
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            var fileContent = File.ReadAllBytes(resourcePath);

            return new InlineResourceResult(fileContent, HttpResponseStatusCode.Ok);

        }


    }
}
