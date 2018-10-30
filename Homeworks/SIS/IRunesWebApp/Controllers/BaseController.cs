namespace IRunesWebApp.Controllers
{
    using System.Linq;
    using System.Runtime.CompilerServices;
    using SIS.Framework.ActionResults;
    using SIS.Framework.Controllers;
    using SIS.Framework.Services.Contracts;

    public abstract class BaseController : Controller
    {
        protected BaseController(IUserCookieService userCookieService)
        {
            this.UserCookieService = userCookieService;
        }
        
        protected IUserCookieService UserCookieService { get; }

        protected override IViewable View([CallerMemberName] string viewName = "")
        {
            var userIdentity = this.Identity;
            //this.Model.Data["IsAdmin"] = "none";

            if (this.IsAuthenticated())
            {
                this.Model.Data["LogIn"] = "block;";
                this.Model.Data["LoggedOut"] = "none;";
                //if (userIdentity.Roles.Contains("Admin"))
                //{
                //    this.Model.Data["IsAdmin"] = "block";
                //}
            }
            else
            {
                this.Model.Data["LogIn"] = "none;";
                this.Model.Data["LoggedOut"] = "block;";
            }
            

            return base.View(viewName);
        }
        
    }
}
