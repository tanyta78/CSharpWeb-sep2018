﻿namespace MishMashWebApp.Controllers
{
    using System.Linq;
    using SIS.HTTP.Responses.Contracts;
    using ViewModels.Channel;
    using ViewModels.Home;

    public class HomeController : BaseController
    {

        public IHttpResponse Index()
        {
            var user = this.Db.Users.FirstOrDefault(u => u.Username == this.User.Username);

            if (user == null)
            {
                return this.View();
            }

            var yourChannels = this.Db.Channels.Where(x => x.Followers.Any(f => f.User.Username == this.User.Username)).Select(c =>
                  new ChannelViewModel()
                  {
                      Id = c.Id,
                      Name = c.Name,
                      Type = c.Type,
                      FollowersCount = c.Followers.Count()
                  }).ToList();

            var myTags = this.Db.Channels
                .Where(x => x.Followers.Any(f => f.User.Username == this.User.Username))
                .SelectMany(x => x.Tags.Select(t => t.TagId)).ToList();

            var suggested = this.Db.Channels.Where(c => c.Followers.All(i => i.User.Username != this.User.Username) && c.Tags.Any(t => myTags.Contains(t.TagId))).Select(ch => new ChannelViewModel()
            {
                Id = ch.Id,
                Name = ch.Name,
                Type = ch.Type,
                FollowersCount = ch.Followers.Count()
            }).ToList();

            var ids = yourChannels.Select(x => x.Id).ToList();
            ids = ids.Concat(suggested.Select(y => y.Id).ToList()).ToList();
            ids = ids.Distinct().ToList();

            var other = this.Db.Channels.Where(c => !ids.Contains(c.Id)).Select(ch => new ChannelViewModel()
            {
                Id = ch.Id,
                Name = ch.Name,
                Type = ch.Type,
                FollowersCount = ch.Followers.Count()
            }).ToList();

            var homeTestViewModel = new HomeTestViewModel()
            {
                Username = this.User.Username,
                YourChannels = yourChannels,
                Suggested = suggested,
                SeeOther = other,
                UserRole = this.User.Role
            };

            return this.View("Home/HomeTest", homeTestViewModel);

        }
    }
}
