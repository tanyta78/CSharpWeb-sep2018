namespace IRunesWebApp.Controllers
{
    using Models;
    using Services;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;
    using System;
    using System.Linq;

    public class AccountController : BaseController
    {
        private readonly IHashService hashService;

        public AccountController()
        {
            this.hashService = new HashService();
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            return this.View("Users/Register");
        }

        public IHttpResponse DoRegister(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();
            var confirmPassword = request.FormData["confirmPassword"].ToString();
            var email = request.FormData["email"].ToString();

            if (password != confirmPassword)
            {
                return this.BadRequestError("Passwords do not match!");
            }

            //2. GENERATE HASH PASSWORD
            var hashedPassword = this.hashService.Hash(password);

            //3. CREATE USER
            var user = new User
            {
                Email = email,
                Username = username,
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

            var response = this.SignInUser(username, request);

            //4. REDIRECT TO HOME PAGE
            return response;
        }

        public IHttpResponse Login(IHttpRequest request)
        {
            return this.View("Users/Login");
        }

        public IHttpResponse DoLogin(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();

            var hashedPassword = this.hashService.Hash(password);
            //1.Validate user exist and pass is correct
            var user = this.Db.Users.FirstOrDefault(u => u.Username == username && u.Password == hashedPassword);

            if (user == null)
            {
                return new RedirectResult("/Users/Login");
            }

            //2.Save session with the user
            var response = this.SignInUser(user.Username, request);

            //4. REDIRECT TO HOME PAGE
            return response;
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            request.Session.ClearParameters();
            var response = new RedirectResult("/");
            if (!request.Cookies.ContainsCookie(".auth-irunes")) return null;
            var cookie = request.Cookies.GetCookie(".auth-irunes");
            cookie.Delete();
            response.Cookies.Add(cookie);
            return response;
        }

    }
}
