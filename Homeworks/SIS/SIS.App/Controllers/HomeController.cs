﻿namespace SIS.App.Controllers
{
    using Framework.ActionResults;
    using Framework.Controllers;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
