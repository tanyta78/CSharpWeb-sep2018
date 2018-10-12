namespace CakesWebApp.Controllers
{
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;

    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            return this.View("Index");
        }

        public IHttpResponse WelcomeUser()
        {
            return new HtmlResult($"<h1>Hello, {this.GetUsername()}</h1",HttpResponseStatusCode.Ok);
        }
    }
}
