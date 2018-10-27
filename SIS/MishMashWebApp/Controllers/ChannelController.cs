namespace MishMashWebApp.Controllers
{
    using System.Linq;
    using Models;
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
                Tags = x.Tags.Select(t => t.Tag.Name),
                Description = x.Description,
                FollowersCount = x.Followers.Count
            }).FirstOrDefault();

            return this.View("Channel/Details", channelViewModel);
        }

        [HttpGet("/channels/followed")]
        public IHttpResponse Followed()
        {
            if (this.User == null)
            {
                return this.Redirect("/users/login");
            }

            var myChannels = this.Db.Channels
                .Where(c => c.Followers.Any(x => x.User.Username == this.User))
                .Select(c => new ChannelViewModel()
                {
                    Type = c.Type,
                    Name = c.Name,
                    FollowersCount = c.Followers.Count,
                    Id = c.Id
                });

            var folowedChannelsViewModel = new FollowedChannelsViewModel()
            {
                FollowingChannels = myChannels
            };

            return this.View("Channel/Followed", folowedChannelsViewModel);
        }

        [HttpGet("/channels/unfollow")]
        public IHttpResponse Unfollow(int id)
        {
            var user = this.Db.Users.FirstOrDefault(u => u.Username == this.User);

            if (user == null)
            {
                return this.Redirect("/users/login");
            }

            var userInChannel = this.Db.UsersInChannels.FirstOrDefault(x => x.UserId == user.Id && x.ChannelId == id);

            if (userInChannel != null)
            {
                this.Db.UsersInChannels.Remove(userInChannel);
                this.Db.SaveChanges();
            }

            return this.Redirect("/channels/followed");

        }

        [HttpGet("/channels/follow")]
        public IHttpResponse Follow(int id)
        {
            var user = this.Db.Users.FirstOrDefault(u => u.Username == this.User);

            if (user == null)
            {
                return this.Redirect("/users/login");
            }

            if (!this.Db.UsersInChannels.Any(x => x.UserId == user.Id && x.ChannelId == id))
            {
                this.Db.UsersInChannels.Add(new UsersInChannel()
                {
                    ChannelId = id,
                    UserId = user.Id
                });

                this.Db.SaveChanges();
            }

            return this.Redirect("/channels/followed");
        }
    }
}
