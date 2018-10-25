namespace IRunesWebApp.Controllers
{
    using SIS.Framework.Controllers;
    using SIS.Framework.Services.Contracts;

    public abstract class BaseController : Controller
    {
        protected BaseController(IUserCookieService userCookieService)
        {
            this.UserCookieService = userCookieService;
        }
        
        protected IUserCookieService UserCookieService { get; }
        
    }
}
