namespace AirportWebApp.Controllers
{
    using System;
    using System.Linq;
   using Models;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Services;
    using ViewModels.Tickets;
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
                x.Email == model.Email.Trim() &&
                x.Password == hashedPassword);

            if (user == null)
            {
                return this.BadRequestErrorWithView("Invalid email or password.");
            }

            //TODO Change with other profile info if needed
            var mvcUser = new MvcUserInfo
            {
                Username = user.Username,
                Role = user.Role.ToString(),
                Info = user.Email,
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
            if (string.IsNullOrWhiteSpace(model.Username) || model.Username.Trim().Length < 4)
            {
                return this.BadRequestErrorWithView("Please provide valid username with length of 4 or more characters.");
            }

            if (string.IsNullOrWhiteSpace(model.Email) || model.Email.Trim().Length < 4)
            {
                return this.BadRequestErrorWithView("Please provide valid email ");
            }

            if (this.Db.Users.Any(x => x.Email == model.Email.Trim()))
            {
                return this.BadRequestErrorWithView("User already exists.");
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
                Email = model.Email.Trim(),
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
        public IHttpResponse Cart()
        {
            var userId = this.Db.Users.First(u => u.Email == this.User.Info).Id;
            var tickets = this.Db.Tickets.Where(t => t.IsPaid == false && t.CustomerId == userId).Select(t =>new TicketDetailsViewModel()
            {
                Id = t.Id,
                Destination = t.Seat.Flight.Destination,
                Origin = t.Seat.Flight.Origin,
                Date = t.Seat.Flight.Date,
                Time = t.Seat.Flight.Time,
                Quantity = t.Quantity,
                Class = t.Seat.Class,
                Price = t.Seat.Price,
                ImageUrl = t.Seat.Flight.ImageUrl
            }).ToList();

            var model = new CartViewModel()
            {
                BookedTicketDetails = tickets
            };
            
            return this.View(model);
        }

        [Authorize()]
        public IHttpResponse Profile() 
        {
            var userId = this.Db.Users.First(u => u.Email == this.User.Info).Id;
            var tickets = this.Db.Tickets.Where(t => t.IsPaid == true && t.CustomerId == userId).Select(t =>new TicketDetailsViewModel()
            {
                Id = t.Id,
                Destination = t.Seat.Flight.Destination,
                Origin = t.Seat.Flight.Origin,
                Date = t.Seat.Flight.Date,
                Time = t.Seat.Flight.Time,
                Quantity = t.Quantity,
                Class = t.Seat.Class,
                Price = t.Seat.Price,
                ImageUrl = t.Seat.Flight.ImageUrl
            }).ToList();

           var model = new ProfileViewModel()
            {
                MyFlightTicketDetails = tickets
            };

            return this.View(model);
        }
    }
}
