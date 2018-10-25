namespace IRunesWebApp.Controllers
{
    using SIS.Framework.ActionResults;
    using SIS.Framework.Attributes.Methods;
    using SIS.Framework.Services.Contracts;

    public class HomeController : BaseController
    {

        public HomeController(IUserCookieService userCookieService) : base(userCookieService)
        {

        }

        [HttpGet]
        public IActionResult Index()
        {
            if (this.IsAuthenticated())
            {
                this.Model.Data["username"] = this.Identity.Username;
                return this.View("Welcome");
            }

            return this.View();
        }
    }
}
