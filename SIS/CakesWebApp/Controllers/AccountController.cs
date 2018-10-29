namespace CakesWebApp.Controllers
{
    using Models;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Services;
    using System;
    using System.Linq;
    using ViewModels.Account;

    public class AccountController : BaseController
    {
        private readonly IHashService hashService;

        public AccountController(IHashService hashService)
        {
            this.hashService = hashService;
        }

        [HttpGet("/account/register")]
        public IHttpResponse Register()
        {
            return this.View("Register");
        }

        [HttpPost("/account/register")]
        public IHttpResponse DoRegister(DoRegisterInputModel model)
        {
            //1.VALIDATE INPUT
            if (string.IsNullOrWhiteSpace(model.Username) || model.Username.Trim().Length < 4)
            {
                return this.BadRequestError("Please provide valid user name with length of 4 or more characters");
            }

            if (this.Db.Users.Any(x => x.Username == model.Username.Trim()))
            {
                return this.BadRequestError("User with the same name already exist!");
            }

            if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Length < 6)
            {
                return this.BadRequestError("Please provide password  with length of 6 or more characters");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return this.BadRequestError("Passwords do not match!");
            }

            //2. GENERATE HASH PASSWORD
            var hashedPassword = this.hashService.Hash(model.Password);

            //3. CREATE USER
            var user = new User
            {
                Name = model.Username.Trim(),
                Username = model.Username.Trim(),
                Password = hashedPassword
            };

            this.Db.Users.Add(user);
            try
            {
                this.Db.SaveChanges();
            }
            catch (Exception e)
            {
                //TODO: log error
                return this.ServerError(e.Message);
            }

            //TODO: LOGIN

            //4. REDIRECT TO HOME PAGE
            return this.Redirect("/");
        }

        [HttpGet("/account/login")]
        public IHttpResponse Login()
        {
            return this.View("Login");
        }

        [HttpPost("/account/login")]
        public IHttpResponse DoLogin(DoLoginInputModel model)
        {
            var hashedPassword = this.hashService.Hash(model.Password);

            //1.Validate user exist and pass is correct
            var user = this.Db.Users.FirstOrDefault(u => u.Username == model.Username.Trim() && u.Password == hashedPassword);

            if (user == null)
            {
                return this.BadRequestError("Invalid username or password");
            }

            //2.Save cookie/session with the user
            var mvcUser = new MvcUserInfo{Username = user.Username};
            var cookieContent = this.UserCookieService.GetUserCookie(mvcUser);
            this.Response.Cookies.Add(new HttpCookie(".auth-app", cookieContent, 7));

            //4. REDIRECT TO HOME PAGE
            return this.Redirect("/");
        }

        [HttpGet("/account/logout")]
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
