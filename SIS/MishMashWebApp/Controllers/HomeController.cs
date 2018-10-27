namespace MishMashWebApp.Controllers
{
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework;

    public class HomeController : BaseController
    {
        [HttpGet("/home/index")]
        public IHttpResponse Index()
        {
           // return this.View(this.User!=null ? "Home/Profile" : "Home/Index");
            return this.View(this.User!=null ? "Home/HomeTest" : "Home/Index");

        }

        [HttpGet("/")]
        public IHttpResponse RootIndex()
        {
           // return this.View(this.User!=null ? "Home/Profile" : "Home/Index");
            return this.View(this.User!=null ? "Home/HomeTest" : "Home/Index");

        }
    }
}
