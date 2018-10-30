namespace TorshiaWebApp.Controllers
{
    using System;
    using Models;
    using Services.Contracts;
    using SIS.Framework.ActionResults;
    using SIS.Framework.Attributes.Methods;
    using SIS.Framework.Security;
    using SIS.Framework.Services.Contracts;
    using ViewModels;

    public class UsersController : BaseController
    {

        public UsersController(IUserService userService, IUserCookieService cookieService) : base(cookieService)
        {
            this.UserService = userService;
        }

        public IUserService UserService { get; }

        [HttpGet]
        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            Console.WriteLine(model.ConfirmPassword);
            Console.WriteLine(model.Username);
            Console.WriteLine(model.Password);
            Console.WriteLine(model.Email);


            if (!this.ModelState.IsValid.HasValue || !this.ModelState.IsValid.Value)
            {
                return this.Register();
            }

            if (model.Password != model.ConfirmPassword)
            {
                this.Model.Data["Error"] = "Passwords do not match";
                return this.Register();
            }

            if (this.UserService.IsUserExistByUsernameAndPassword(model.Username, model.Password))
            {
                this.Model.Data["Error"] = "User already exist";
                return this.Register();
            }

            var user = this.UserService.RegisterUser(model.Username, model.Password, model.ConfirmPassword,
                model.Email);

            if (user != null)
            {
                string[] roles = new[] {user.Role.ToString()};
                this.SignIn(new IdentityUser { Username = model.Username, Roles = roles});
                this.Model.Data["username"] = model.Username;

                return this.RedirectToAction("/");
            }

            this.Model.Data["Error"] = "Invalid input data";
            return this.Register();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!this.ModelState.IsValid.HasValue || !this.ModelState.IsValid.Value)
            {
                return this.RedirectToAction("/Users/Login");
            }


            //1.Validate user exist and pass is correct
            var user = this.UserService.GetUser(model.Username.Trim(), model.Password);

            if (user != null)
            {
                string[] roles = new[] {user.Role.ToString()};
                this.SignIn(new IdentityUser() { Username = model.Username, Roles = roles});
                return this.RedirectToAction("/");
            }

            this.Model.Data["Error"] = "Invalid credentials";

            return this.Login();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            this.SignOut();

            return this.RedirectToAction("/");
        }

    }
}
