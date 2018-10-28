namespace MishMashWebApp.ViewModels.Home
{
    using System.Collections.Generic;
    using Channel;

    public class HomeTestViewModel
    {
        public string Username { get; set; }
        public string UserRole { get; set; }
        public IEnumerable<ChannelViewModel> YourChannels { get; set; } = new List<ChannelViewModel>();
        public IEnumerable<ChannelViewModel> Suggested { get; set; } = new List<ChannelViewModel>();
        public IEnumerable<ChannelViewModel> SeeOther { get; set; } = new List<ChannelViewModel>();
    }
}
