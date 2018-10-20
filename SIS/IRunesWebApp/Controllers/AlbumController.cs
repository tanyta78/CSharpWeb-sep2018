namespace IRunesWebApp.Controllers
{
    using Microsoft.EntityFrameworkCore;
    using Models;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Logger;
    using SIS.WebServer.Results;
    using System;
    using System.Linq;
    using ViewModels.Albums;

    public class AlbumController : BaseController
    {
        private readonly ILogger logger;

        public AlbumController(ILogger logger)
        {
            this.logger = logger;
        }

        [HttpGet("/Albums/All")]
        public IHttpResponse All()
        {
            if (!this.IsAuthenticated())
            {
                return this.Redirect("/Users/Login");
            }

            var allAlbums = this.Db.Albums.ToList();
            var model = new AllAlbumsViewModel
            {
                Count = allAlbums.Count(),
                AllAlbums = allAlbums
            };
            return this.View("Albums/All", model);
        }

        [HttpGet("/Albums/Create")]
        public IHttpResponse Create()
        {
            if (!this.IsAuthenticated())
            {
                return this.Redirect("/Users/Login");
            }


            return this.View("Albums/Create");
        }

        [HttpPost("/Albums/Create")]
        public IHttpResponse DoCreate(DoCreateInputModel model)
        {
            if (!this.IsAuthenticated())
            {
                return this.Redirect("/Users/Login");
            }

            var album = new Album
            {
                Cover = model.Cover,
                Name = model.Name.Trim()
            };

            this.Db.Albums.Add(album);
            try
            {
                this.Db.SaveChanges();
            }
            catch (Exception e)
            {
                //TODO: log error
                return this.ServerError(e.Message);
            }


            return this.Redirect("/Albums/All");
        }

        [HttpGet("/Albums/Details")]
        public IHttpResponse Details()
        {
            if (!this.IsAuthenticated())
            {
                return this.Redirect("/Users/Login");
            }


            var id = this.Request.QueryData["id"].ToString().ToUpper();
            var album = this.Db.Albums.Include(x => x.Tracks).ThenInclude(x => x.Track).FirstOrDefault(a => a.Id.ToString() == id);
            var model = new AlbumViewModel();
            if (album == null)
            {
                return new BadRequestResult($"There are currently no album with {id}.");
            }
            else
            {

                model.Name = album.Name;
                model.Id = album.Id.ToString().ToUpper();
                model.Cover = album.Cover;
                model.Price = decimal.Parse(album.Cover);

                album.Tracks.ToList().ForEach(t => t.Id.ToString().ToUpper());
                model.Tracks = album.Tracks.ToList();
                model.Count = model.Tracks.Count();

            }

            return this.View("Albums/Details", model);
        }
    }
}
