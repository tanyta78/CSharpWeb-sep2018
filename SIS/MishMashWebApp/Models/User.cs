namespace MishMashWebApp.Models
{
    using System.Collections.Generic;

    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public virtual ICollection<UsersInChannel> FollowedChannels { get; set; } = new HashSet<UsersInChannel>();

        public Role Role { get; set; }
    }
}