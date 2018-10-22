namespace IRunesWebApp.Controllers
{
    using SIS.Framework.ActionResults;
    using SIS.Framework.Controllers;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //if (this.IsAuthenticated(request))
            //{
            //    var username = request.Session.GetParameter("username");
            //    this.ViewBag["username"] = username.ToString();
            //    return this.ViewMethod("Welcome");
            //}

            return this.View();
        }
    }
}
