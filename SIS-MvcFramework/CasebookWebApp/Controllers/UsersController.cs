namespace CasebookWebApp.Controllers
{
    using System;
    using System.Linq;
    using Models;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Services;
    using ViewModels.Home;
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
                Info = user.Gender,
            };
            var cookieContent = this.UserCookieService.GetUserCookie(mvcUser);

            var cookie = new HttpCookie(".auth-cakes", cookieContent, 7) { HttpOnly = true };
            this.Response.Cookies.Add(cookie);
            return this.Redirect("/home/home");
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
            if (string.IsNullOrWhiteSpace(model.Username) || model.Username.Trim().Length < 4)
            {
                return this.BadRequestErrorWithView("Please provide valid username with length of 4 or more characters.");
            }

            if (this.Db.Users.Any(x => x.Username == model.Username.Trim()))
            {
                return this.BadRequestErrorWithView("User with the same name already exists.");
            }

            if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Length < 6)
            {
                return this.BadRequestErrorWithView("Please provide password of length 6 or more.");
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
                Gender = model.Gender.Trim(),
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

        [Authorize()]
        [HttpPost()]
        public IHttpResponse AddFriend(int id)
        {
            var friend = this.Db.Users.FirstOrDefault(u => u.Id == id);

            if (friend == null)
            {
                return this.BadRequestError("User do not exist.");
            }

            var currentUser = this.Db.Users.First(u => u.Username == this.User.Username);

            if (currentUser.Friends.Contains(friend) || currentUser.Id == friend.Id)
            {
                return this.Redirect("/users/friends");

            }

            currentUser.Friends.Add(friend);
            this.Db.SaveChanges();

            return this.Redirect("/users/friends");
        }

        [Authorize()]
        [HttpPost()]
        public IHttpResponse RemoveFriend(int id)
        {
            var friend = this.Db.Users.FirstOrDefault(u => u.Id == id);

            if (friend == null)
            {
                return this.BadRequestError("User do not exist.");
            }

            var currentUser = this.Db.Users.First(u => u.Username == this.User.Username);

            if (!currentUser.Friends.Contains(friend) || currentUser.Id == friend.Id)
            {
                return this.Redirect("/users/friends");

            }

            currentUser.Friends.Remove(friend);
            this.Db.SaveChanges();

            return this.Redirect("/users/friends");
        }

        [Authorize()]
        public IHttpResponse Friends()
        {
            var currentUser = this.Db.Users.First(u => u.Username == this.User.Username);
            var friendList = currentUser.Friends.Select(
                x => new UserViewModel
                {
                    Id = x.Id,
                    Username = x.Username,
                }).ToList();

            var model = new FriendsViewModel
            {
                Friends = friendList,
            };

            return this.View(model);
        }

        [Authorize()]
        public IHttpResponse Profile(int id)
        {
            var user = this.Db.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return this.BadRequestError("User do not exist.");
            }

            var model = new UserViewModel()
            {
                Id = user.Id,
                Gender = user.Gender.ToLower(),
                Username = user.Username
            };


            return this.View(model);
        }
    }
}
