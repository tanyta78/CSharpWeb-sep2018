namespace MishMashWebApp.Models
{
    using System.Collections.Generic;

    public class Channel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ChannelType Type { get; set; }

        public virtual ICollection<ChannelTag> Tags { get; set; } = new HashSet<ChannelTag>();

        public virtual ICollection<UsersInChannel> Followers { get; set; } = new HashSet<UsersInChannel>();
    }
}