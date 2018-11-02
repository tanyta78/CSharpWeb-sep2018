namespace GameStoreWebApp.Controllers
{
    using SIS.HTTP.Responses;

    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            return this.Redirect("/Games/All");
        }
    }
}
