namespace IRunesWebApp.Controllers
{
    using Microsoft.EntityFrameworkCore;
    using Models;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Logger;
    using System;
    using System.Linq;
    using ViewModels.Album;

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

            var allAlbums = this.Db.Albums.Select(a => $"<a href=\"/Albums/Details?id={a.Id.ToString().ToUpper()}\"/>{a.Name}</a> </br>");
            var allAlbumsString = String.Join(Environment.NewLine, allAlbums);

            this.ViewBag["allAlbums"] = string.IsNullOrWhiteSpace(allAlbumsString)
                ? "There are currently no albums."
                : allAlbumsString;
            return this.View("Albums/All");
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

        [HttpGet("/albums/details")]
        public IHttpResponse Details()
        {
            if (!this.IsAuthenticated())
            {
                return this.Redirect("/Users/Login");
            }


            var id = this.Request.QueryData["id"].ToString().ToUpper();
            var album = this.Db.Albums.Include(x => x.Tracks).ThenInclude(x => x.Track).FirstOrDefault(a => a.Id.ToString() == id);

            if (album == null)
            {
                this.ViewBag["albumDetails"] = $"There are currently no album with {id}.";
            }
            else
            {
                var albumDetailsAsString = $"<img src={album.Cover} alt={album.Name} class=\"img-fluid\" > " +
                                           $"<h4 class=\"text-center\">Album Name:{album.Name} </h4>" +
                                           $"<h4 class=\"text-center\">Album Price:$ {album.Price:f2}</h4>";
                var createTrack = $"<a href=\"/Tracks/Create?albumId={id}\" class=\"btn btn-success\">Create Track</a>";
                this.ViewBag["albumDetails"] = albumDetailsAsString;
                this.ViewBag["createTrack"] = createTrack;


                var albumTracksAsString = album.Tracks.Select(t => $"<li><a href=\"/Tracks/Details?albumId={id}&trackId={t.Track.Id.ToString().ToUpper()}\"/>{t.Track.Name}</a></li>");

                this.ViewBag["albumTracks"] = album.Tracks.Count == 0
                    ? "There are no tracks in album"
                    : $"<ol>{string.Join(Environment.NewLine, albumTracksAsString)}</ol>";

            }

            return this.View("Albums/Details");
        }
    }
}
