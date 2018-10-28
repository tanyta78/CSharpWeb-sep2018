namespace MishMashWebApp.Controllers
{
    using System;
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

        [HttpGet("/channels/create")]
        public IHttpResponse Create()
        {
            var user = this.Db.Users.FirstOrDefault(u => u.Username == this.User);

            if (user == null || user.Role != Role.Admin)
            {
                return this.BadRequestError("You do not have permission to access this functionality!");
            }

            return this.View("Channel/Create");
        }

        [HttpPost("/channels/create")]
        public IHttpResponse Create(CreateChannelsInputModel model)
        {
            var user = this.Db.Users.FirstOrDefault(u => u.Username == this.User);

            if (user == null || user.Role != Role.Admin)
            {
                return this.BadRequestError("You do not have permission to access this functionality!");
            }

            if (!Enum.TryParse(model.Type, true, out ChannelType type))
            {
                return this.BadRequestError("Invalid channel type.");
            };

            var channel = new Channel()
            {
                Name = model.Name,
                Description = model.Description,
                Type = type
            };

            if (!string.IsNullOrWhiteSpace(model.Tags))
            {
                var tags = model.Tags.Split(',', ';',StringSplitOptions.RemoveEmptyEntries);

                foreach (var tag in tags)
                {
                    var dbTag = this.Db.Tags.FirstOrDefault(x => x.Name == tag.Trim());

                    if (dbTag == null)
                    {
                        dbTag=new Tag(){Name = tag.Trim()};
                        this.Db.Tags.Add(dbTag);
                        this.Db.SaveChanges();
                    }

                    channel.Tags.Add(new ChannelTag()
                    {
                        TagId = dbTag.Id
                    });
                }
            }

          this.Db.Channels.Add(channel);
            this.Db.SaveChanges();

            return this.Redirect("/channels/details?id="+channel.Id);
        }
    }
}
