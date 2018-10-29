namespace CakesWebApp.Controllers
{
    using SIS.HTTP.Responses.Contracts;
    using System.Collections.Generic;
    using SIS.MvcFramework;
    using ViewModels.Home;

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
           return this.View("WelcomeUser", new HelloUserViewModel{Username = this.User.Username});
        }
    }
}
