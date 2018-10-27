namespace MishMashWebApp.Controllers
{
    using System.Linq;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework;
    using ViewModels.Channel;

    public class ChannelController : BaseController
    {
        [HttpGet("/channels/details")]
        public IHttpResponse Details(int id)
        {
            if (this.User == null)
            {
                return this.Redirect("/users/login");
            }

            var channelViewModel = this.Db.Channels.Where(c => c.Id == id).Select(x => new ChannelViewModel
            {
                Type = x.Type,
                Name = x.Name,
                Tags = x.Tags.Select(t=>t.Tag.Name),
                Description = x.Description,
                FollowersCount = x.Followers.Count
            }).FirstOrDefault();

            return this.View("Channel/Details", channelViewModel);
        }

        [HttpGet("/channels/followed")]
        public IHttpResponse MyChannels()
        {
            if (this.User == null)
            {
                return this.Redirect("/users/login");
            }

           
        }
    }
}
