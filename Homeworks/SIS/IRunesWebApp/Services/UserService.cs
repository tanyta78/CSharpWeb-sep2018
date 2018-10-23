namespace IRunesWebApp.Services
{
    using System;
    using System.Linq;
    using Contracts;
    using Data;
    using Models;
    using SIS.Framework.Services.Contracts;

    public class UserService : IUserService
    {
        private readonly IRunesDbContext db;
        private readonly IHashService hashService;

        public UserService(IRunesDbContext context, IHashService hashService)
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

        public bool RegisterUser(string username, string password, string confirmPassword, string email)
        {
            if (password != confirmPassword)
            {
                return false;
            }

           if (this.IsUserExistByUsernameAndPassword(username, password))
            {
                return false;
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

            this.db.Users.Add(user);
            try
            {
                this.db.SaveChanges();
            }
            catch (Exception e)
            {
                //TODO: handle error
                return false;
            }

            return true;
        }
    }
}