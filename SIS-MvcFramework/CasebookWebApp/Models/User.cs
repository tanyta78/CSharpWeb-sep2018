namespace CasebookWebApp.Models
{
    using System.Collections.Generic;

    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Gender { get; set; }

        public UserRole Role { get; set; }

        public virtual ICollection<User> Friends { get; set; }
    }
}
