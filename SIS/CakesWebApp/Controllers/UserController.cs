namespace CakesWebApp.Controllers
{
    using System.Linq;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework;
    using ViewModels.User;

    public class UserController : BaseController
    {
        [HttpGet("/user/profile")]
        public IHttpResponse Profile()
        {
           var viewModel =  this.Db.Users.Where(x => x.Username == this.User)
                .Select(x=>new ProfileViewModel
                  {
                      Username = x.Username,
                      DateOfRegister = x.DateOfRegistration,
                      OrdersCount = x.Orders.Count
                  }).FirstOrDefault();

            if (viewModel == null)
            {
                return this.BadRequestError("Profile page not accessible for this user.");
            }

            return this.View("Profile",viewModel);
        }
    }
}
