namespace TorshiaWebApp.Controllers
{
    using Services;
    using Services.Contracts;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Services;
    using ViewModels;
    using ViewModels.Users;

    public class UsersController : BaseController
    {

        public UsersController(UserService userService, HashService hashService)
        {
            this.UserService = userService;
           
        }

        public UserService UserService { get; }
       

        public IHttpResponse Register()
        {
            return this.View();
        }

        [HttpPost]
        public IHttpResponse Register(RegisterViewModel model)
        {

            if (string.IsNullOrWhiteSpace(model.Username) || model.Username.Trim().Length < 3)
            {
                return this.BadRequestErrorWithView("Please provide valid user name with length of 3 or more characters");
            }

            if (string.IsNullOrWhiteSpace(model.Email) || model.Email.Trim().Length < 3)
            {
                return this.BadRequestErrorWithView("Please provide valid email with length of 3 or more characters");
            }

            if (this.UserService.IsUserExistByUsername(model.Username))
            {
                return this.BadRequestErrorWithView("User with the same name already exist!");
            }

            if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Length < 3)
            {
                return this.BadRequestErrorWithView("Please provide password  with length of 3 or more characters");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return this.BadRequestErrorWithView("Passwords do not match!");
            }

            this.UserService.RegisterUser(model.Username, model.Password, model.Email);


            return this.Redirect("/Users/Login");
        }

        public IHttpResponse Login()
        {
            return this.View();
        }

        [HttpPost]
        public IHttpResponse Login(LoginViewModel model)
        {
           var user = this.UserService.GetUser(model.Username.Trim(), model.Password);

            if (user == null)
            {
                return this.BadRequestErrorWithView("Invalid username or password");
            }

            //2.Save cookie/session with the user
            var mvcUser = new MvcUserInfo { Username = user.Username, Role = user.Role.ToString(), Info = user.Email };

            var cookieContent = this.UserCookieService.GetUserCookie(mvcUser);
            this.Response.Cookies.Add(new HttpCookie(".auth-app", cookieContent, 7));

            //4. REDIRECT TO HOME PAGE
            return this.Redirect("/Home/Index");

        }


        public IHttpResponse Logout()
        {
            if (!this.Request.Cookies.ContainsCookie(".auth-app"))
            {
                return this.Redirect("/");
            }

            var cookie = this.Request.Cookies.GetCookie(".auth-app");
            cookie.Delete();
            this.Response.Cookies.Add(cookie);
            return this.Redirect("/"); ;
        }

    }
}
