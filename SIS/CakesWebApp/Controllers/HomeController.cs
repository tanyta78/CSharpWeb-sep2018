namespace CakesWebApp.Controllers
{
    using SIS.HTTP.Responses.Contracts;
    using System.Collections.Generic;

    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            return this.View("Index");
        }

        public IHttpResponse WelcomeUser()
        {
            var viewBag = new Dictionary<string, string>
            {
                {"Username", this.GetUsername()}
            };
            return this.View("WelcomeUser", viewBag);
        }
    }
}
