namespace TorshiaWebApp.Services
{
    using System;
    using System.Linq;
    using Contracts;
    using Data;
    using Models;
    using SIS.Framework.Services.Contracts;

    public class UserService : IUserService
    {
        private readonly TorshiaDbContext db;
        private readonly IHashService hashService;

        public UserService(TorshiaDbContext context, IHashService hashService)
        {
            this.db = context;
            this.hashService = hashService;
        }

        public bool IsUserExistByUsernameAndPassword(string username, string password)
        {
            return this.GetUser(username, password) != null;
        }

        public User GetUser(string username, string password)
        {
            var hashedPassword = this.hashService.Hash(password);

            var user = this.db.Users.FirstOrDefault(u => u.Username == username && u.Password == hashedPassword);

            return user;
        }

        public User RegisterUser(string username, string password, string confirmPassword, string email)
        {
            //1. GENERATE HASH PASSWORD
            var hashedPassword = this.hashService.Hash(password);

            //2. CREATE USER
            var user = new User
            {
                Email = email,
                Username = username,
                Password = hashedPassword,
                Role = Role.User
            };

           
            if (!this.db.Users.Any())
            {
                user.Role = Role.Admin;
            }

            this.db.Users.Add(user);
            try
            {
                this.db.SaveChanges();
            }
            catch (Exception e)
            {
                //TODO: handle error
                Console.WriteLine(e.Message);
                return null;
            }

            return user;
        }

        public User GetUserByUsername(string identityUsername)
        {
            return this.db.Users.FirstOrDefault(u => u.Username == identityUsername);
        }
    }
}