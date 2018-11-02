namespace CasebookWebApp.Controllers
{
    using System.Linq;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;
    using ViewModels.Home;

    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            if (this.User.IsLoggedIn)
            {
                var currentUser = this.Db.Users.First(u => u.Username == this.User.Username);
                var allUsersList = this.Db.Users.Where(u=>!currentUser.Friends.Contains(u) && u.Id!=currentUser.Id).ToList().Select(
                    x => new UserViewModel
                    {
                        Id = x.Id,
                        Username = x.Username,
                        Gender = x.Gender.ToLower()
                    }).ToList();
                var model = new IndexViewModel
                {
                    Users = allUsersList,
                };

                return this.View("Home/IndexLoggedIn", model);
            }

            return this.View();
        }

        [Authorize()]
        public IHttpResponse Home()
        {
            return this.Redirect("/");
        }
    }
}
