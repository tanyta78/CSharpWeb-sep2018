﻿namespace IRunesWebApp.Controllers
{
    using Services;
    using SIS.Framework.ActionResults;
    using SIS.Framework.Attributes.Methods;

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
                var username = this.Request.Session.GetParameter("username");
                this.Model["username"] = username.ToString();
                return this.View("Welcome");
            }

            return this.View();
        }
    }
}
