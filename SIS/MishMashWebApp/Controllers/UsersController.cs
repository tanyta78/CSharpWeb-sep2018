﻿namespace MishMashWebApp.Controllers
{
    using System;
    using System.Linq;
    using Models;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Services;
    using ViewModels.User;

    public class UsersController : BaseController
    {

        private readonly IHashService hashService;

        public UsersController(IHashService hashService)
        {
            this.hashService = hashService;
        }


        public IHttpResponse Register()
        {
            return this.View();
        }

        [HttpPost]
        public IHttpResponse Register(DoRegisterInputModel model)
        {
            //1.VALIDATE INPUT
            if (string.IsNullOrWhiteSpace(model.Username) || model.Username.Trim().Length < 3)
            {
                return this.BadRequestErrorWithView("Please provide valid user name with length of 3 or more characters");
            }

            if (string.IsNullOrWhiteSpace(model.Email) || model.Email.Trim().Length < 3)
            {
                return this.BadRequestErrorWithView("Please provide valid email with length of 3 or more characters");
            }

            if (this.Db.Users.Any(x => x.Username == model.Username.Trim()))
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

            //2. GENERATE HASH PASSWORD
            var hashedPassword = this.hashService.Hash(model.Password);

            var role = Role.User;

            if (!this.Db.Users.Any())
            {
                role = Role.Admin;
            }

            //3. CREATE USER
            var user = new User
            {
                Username = model.Username.Trim(),
                Password = hashedPassword,
                Email = model.Email,
                Role = role
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
            return this.Redirect("/Users/Login");
        }


        public IHttpResponse Login()
        {
            return this.View();
        }

        [HttpPost]
        public IHttpResponse Login(DoLoginInputModel model)
        {
            var hashedPassword = this.hashService.Hash(model.Password);

            //1.Validate user exist and pass is correct
            var user = this.Db.Users.FirstOrDefault(u => u.Username == model.Username.Trim() && u.Password == hashedPassword);

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

