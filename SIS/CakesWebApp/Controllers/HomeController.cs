namespace CakesWebApp.Controllers
{
    using SIS.HTTP.Responses.Contracts;
    using System.Collections.Generic;
    using SIS.MvcFramework;

    public class HomeController : BaseController
    {
        [HttpGet("/")]
        public IHttpResponse Index()
        {
            return this.View("Index");
        }

        [HttpGet("/hello")]
        public IHttpResponse WelcomeUser()
        {
            var viewBag = new Dictionary<string, string>
            {
                {"Username", this.User}
            };
            return this.View("WelcomeUser", viewBag);
        }
    }
}
