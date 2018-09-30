namespace SIS.App
{
    using HTTP.Enums;
    using HTTP.Responses.Contracts;
    using WebServer.Results;

    public class HomeController
    {
        public IHttpResponse Index()
        {
            string content = "<h1>Hello my friend !</h1>";

            return new HtmlResult(content, HttpResponseStatusCode.Ok);
        }
    }
}
