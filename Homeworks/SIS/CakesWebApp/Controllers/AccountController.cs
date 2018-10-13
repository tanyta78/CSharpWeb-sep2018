namespace CakesWebApp.Controllers
{
    using Models;
    using Services;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;
    using System;
    using System.Linq;
    using SIS.HTTP.Cookies;

    public class AccountController : BaseController
    {
        private IHashService hashService;

        public AccountController()
        {
            this.hashService = new HashService();
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            return this.View("Register");
        }

        public IHttpResponse DoRegister(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();
            var confirmPassword = request.FormData["confirmPassword"].ToString();

            //1.VALIDATE INPUT
            if (string.IsNullOrWhiteSpace(username) || username.Length < 4)
            {
                return this.BadRequestError("Please provide valid user name with length of 4 or more characters");
            }

            if (this.Db.Users.Any(x => x.Username == username))
            {
                return this.BadRequestError("User with the same name already exist!");
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                return this.BadRequestError("Please provide password  with length of 6 or more characters");
            }

            if (password != confirmPassword)
            {
                return this.BadRequestError("Passwords do not match!");
            }

            //2. GENERATE HASH PASSWORD
            var hashedPassword = this.hashService.Hash(password);

            //3. CREATE USER
            var user = new User
            {
                Name = username,
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

            //TODO: LOGIN

            //4. REDIRECT TO HOME PAGE
            return new RedirectResult("/");
        }

        public IHttpResponse Login(IHttpRequest request)
        {
            return this.View("Login");
        }

        public IHttpResponse DoLogin(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();

            var hashedPassword = this.hashService.Hash(password);
            //1.Validate user exist and pass is correct
            var user = this.Db.Users.FirstOrDefault(u => u.Username == username && u.Password == hashedPassword);

            if (user==null)
            {
                return this.BadRequestError("Invalid username or password");
            }

            //2.Save cookie/session with the user
            var cookieContent = this.UserCookieService.GetUserCookie(user.Username);
            var response = new RedirectResult("/");
            response.Cookies.Add(new HttpCookie(".auth-cakes",cookieContent,7));

            //4. REDIRECT TO HOME PAGE
            return response;
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            var response =new RedirectResult("/");
            if (!request.Cookies.ContainsCookie(".auth-cakes")) return null;
            var cookie = request.Cookies.GetCookie(".auth-cakes");
            cookie.Delete();
            response.Cookies.Add(cookie);
            return response;
        }

    }
}
