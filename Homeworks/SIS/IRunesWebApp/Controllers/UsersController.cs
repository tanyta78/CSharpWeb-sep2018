namespace IRunesWebApp.Controllers
{
    using System;
    using Services.Contracts;
    using SIS.Framework.ActionResults;
    using SIS.Framework.Attributes.Methods;
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
            Console.WriteLine(model.Confirmpassword);
            Console.WriteLine(model.Username);
            Console.WriteLine(model.Password);
            Console.WriteLine(model.Email);


            if (!this.ModelState.IsValid.HasValue || !this.ModelState.IsValid.Value)
            {
                return this.Register();
            }

            if (this.UserService.RegisterUser(model.Username, model.Password, model.Confirmpassword, model.Email))
            {
                this.SignInUser(model.Username, this.Request);
                this.Model.Data["username"] = model.Username;
              
                return this.RedirectToAction("/");
            }

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
                this.SignInUser(user.Username, this.Request);
                return this.RedirectToAction("/");
            }

            this.Model.Data["Error"] = "Invalid credentials";

            return this.Login();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            this.Request.Session.ClearParameters();

            return this.RedirectToAction("/");
        }

    }
}
