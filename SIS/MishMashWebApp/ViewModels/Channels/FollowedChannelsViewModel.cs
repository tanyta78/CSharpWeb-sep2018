namespace MishMashWebApp.ViewModels.Channel
{
    using System.Collections.Generic;

    public class FollowedChannelsViewModel
    {
        public IEnumerable<ChannelViewModel> FollowingChannels { get; set; } = new List<ChannelViewModel>();
    }
}
