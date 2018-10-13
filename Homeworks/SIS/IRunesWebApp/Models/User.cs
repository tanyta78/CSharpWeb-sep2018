namespace IRunesWebApp.Models
{
    using System;

    public class User : BaseModel<Guid>
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

       //public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
