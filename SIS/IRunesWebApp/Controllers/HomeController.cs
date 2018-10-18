namespace IRunesWebApp.Controllers
{
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework;

    public class HomeController : BaseController
    {
        [HttpGet("/")]
        public IHttpResponse Index()
        {
            if (this.IsAuthenticated())
            {
                var username = this.Request.Session.GetParameter("username");
                this.ViewBag["username"] = username.ToString();
                return this.View("Welcome");
            }

            return this.View("Index");
        }

        [HttpGet("/Home/Index")]
        public IHttpResponse Home()
        {
            return this.Redirect("/");
        }
    }
}
