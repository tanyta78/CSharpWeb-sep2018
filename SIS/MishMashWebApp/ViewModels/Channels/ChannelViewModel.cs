namespace MishMashWebApp.ViewModels.Channel
{
    using System.Collections.Generic;
    using Models;

    public class ChannelViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ChannelType Type { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public string TagsAsString => string.Join(",", this.Tags);

        public int FollowersCount { get; set; }
    }
}
