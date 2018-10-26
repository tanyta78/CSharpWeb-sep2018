namespace MishMashWebApp.Controllers
{
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework;

    public class HomeController : BaseController
    {
        [HttpGet("/Home/Index")]
        public IHttpResponse Index()
        {
            return this.View("Home/Index");
        }

        [HttpGet("/")]
        public IHttpResponse RootIndex()
        {
            return this.View("Home/Index");
        }
    }
}
