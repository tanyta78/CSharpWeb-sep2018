namespace IRunesWebApp.Controllers
{
    using IRunesWebApp.ViewModels.Account;
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
               var model= new DoLoginInputModel
               {
                   Username = username.ToString()
               };
                return this.View("Welcome",model);
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
