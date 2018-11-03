namespace IssueTrackerWebApp.Controllers
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Models;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Services;
    using ViewModels.Users;

    public class UsersController : BaseController
    {
        private readonly IHashService hashService;

        public UsersController(IHashService hashService)
        {
            this.hashService = hashService;
        }

        [Authorize]
        public IHttpResponse Logout()
        {
            if (!this.Request.Cookies.ContainsCookie(".auth-cakes"))
            {
                return this.Redirect("/");
            }

            var cookie = this.Request.Cookies.GetCookie(".auth-cakes");
            cookie.Delete();
            this.Response.Cookies.Add(cookie);
            return this.Redirect("/");
        }

        public IHttpResponse Login()
        {
            if (this.User.IsLoggedIn)
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        [HttpPost]
        public IHttpResponse Login(DoLoginInputModel model)
        {
            var hashedPassword = this.hashService.Hash(model.Password);

            var user = this.Db.Users.FirstOrDefault(x =>
                x.Username == model.Username.Trim() &&
                x.Password == hashedPassword);

            if (user == null)
            {
                return this.BadRequestErrorWithView("Invalid username or password.");
            }

            //TODO Change with other profile info if needed
            var mvcUser = new MvcUserInfo
            {
                Username = user.Username,
                Role = user.Role.ToString(),
                Info = user.FullName,
            };
            var cookieContent = this.UserCookieService.GetUserCookie(mvcUser);

            var cookie = new HttpCookie(".auth-cakes", cookieContent, 7) { HttpOnly = true };
            this.Response.Cookies.Add(cookie);
            return this.Redirect("/");
        }

        public IHttpResponse Register()
        {
            if (this.User.IsLoggedIn)
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        [HttpPost]
        public IHttpResponse Register(DoRegisterInputModel model)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(model.Username) || model.Username.Trim().Length < 5 || model.Username.Trim().Length > 30)
            {
                return this.BadRequestErrorWithView("Please provide valid username with length between 5 and 30 characters.");
            }

            if (string.IsNullOrWhiteSpace(model.FullName) || model.FullName.Trim().Length < 5)
            {
                return this.BadRequestErrorWithView("Please provide valid full name with length of 5 or more characters.");
            }

            if (this.Db.Users.Any(x => x.Username == model.Username.Trim()))
            {
                return this.BadRequestErrorWithView("User with the same name already exists.");
            }


            if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Length < 8)
            {
                return this.BadRequestErrorWithView("Please provide password of length 8 or more.");
            }


            var rgx = new Regex("(?=.*[!@#$%^&*,.])(?=.*[A-Z])(?=.*[0-9]).*");
            if (!rgx.IsMatch(model.Password))
            {
                return this.BadRequestErrorWithView("Please provide password contain a capital letter, a number and one of the following signs: [!@#$%^&*,.]");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return this.BadRequestErrorWithView("Passwords do not match.");
            }

            // Hash password
            var hashedPassword = this.hashService.Hash(model.Password);

            var role = UserRole.User;
            if (!this.Db.Users.Any())
            {
                role = UserRole.Admin;
            }

            // Create user
            var user = new User
            {
                Username = model.Username.Trim(),
                FullName = model.FullName,
                Password = hashedPassword,
                Role = role,
            };
            this.Db.Users.Add(user);

            try
            {
                this.Db.SaveChanges();
            }
            catch (Exception e)
            {

                return this.BadRequestErrorWithView(e.Message);
            }

            // Redirect
            return this.Redirect("/Users/Login");
        }
    }
}
