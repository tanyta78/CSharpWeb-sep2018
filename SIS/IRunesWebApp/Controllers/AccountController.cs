namespace IRunesWebApp.Controllers
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

        [HttpGet("/Users/Register")]
        public IHttpResponse Register()
        {
            return this.View("Users/Register");
        }

        [HttpPost("/Users/Register")]
        public IHttpResponse DoRegister(DoRegisterInputModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                return this.BadRequestError("Passwords do not match!");
            }

            //2. GENERATE HASH PASSWORD
            var hashedPassword = this.hashService.Hash(model.Password);

            //3. CREATE USER
            var user = new User
            {
                Email = model.Email,
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

            //4. REDIRECT TO HOME PAGE
            return this.Redirect("/");
        }

        [HttpGet("/Users/Login")]
        public IHttpResponse Login()
        {
            return this.View("Users/Login");
        }

        [HttpPost("/Users/Login")]
        public IHttpResponse DoLogin(DoLoginInputModel model)
        {
            var hashedPassword = this.hashService.Hash(model.Password);
            //1.Validate user exist and pass is correct
            var user = this.Db.Users.FirstOrDefault(u => u.Username == model.Username.Trim() && u.Password == hashedPassword);

            if (user == null)
            {
                return this.BadRequestError("Invalid username or password");
            }

            //2.Save session/cookie with the user
            this.SignInUser(user.Username);

            //4. REDIRECT TO HOME PAGE
            return this.Redirect("/");
        }

        private void SignInUser(string username)
        {
            this.Request.Session.AddParameter("username", username);
            var cookieContent = this.UserCookieService.GetUserCookie(username);

            this.Response.Cookies.Add(new HttpCookie(".auth-app", cookieContent, 7));

        }

        [HttpGet("/logout")]
        public IHttpResponse Logout()
        {
            this.Request.Session.ClearParameters();

            if (!this.Request.Cookies.ContainsCookie(".auth-app")) return this.Redirect("/");
            var cookie = this.Request.Cookies.GetCookie(".auth-app");
            cookie.Delete();
            this.Response.Cookies.Add(cookie);
            return this.Redirect("/");
        }

    }
}
