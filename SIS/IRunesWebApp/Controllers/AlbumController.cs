namespace IRunesWebApp.Controllers
{
    using Models;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public class AlbumController : BaseController
    {

        public IHttpResponse All(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/Users/Login");
            }

            var allAlbums = this.Db.Albums.Select(a => $"<a href=\"/Albums/Details?id={a.Id.ToString().ToUpper()}\"/>{a.Name}</a> </br>");
            var allAlbumsString = String.Join(Environment.NewLine, allAlbums);
            this.ViewBag["allAlbums"] = string.IsNullOrWhiteSpace(allAlbumsString)
                ? "There are currently no albums."
                : allAlbumsString;
            return this.View("Albums/All");
        }


        public IHttpResponse Create(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/Users/Login");
            }
            return this.View("Albums/Create");
        }

        public IHttpResponse DoCreate(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/Users/Login");
            }
            var name = request.FormData["name"].ToString().Trim();
            var cover = request.FormData["cover"].ToString();

            var album = new Album
            {
                Cover = cover,
                Name = name
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


            return new RedirectResult("/Albums/All");
        }

        public IHttpResponse Details(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/Users/Login");
            }

            var id = request.QueryData["id"].ToString().ToUpper();
            var album = this.Db.Albums.Include(x=>x.Tracks).ThenInclude(x=>x.Track).FirstOrDefault(a => a.Id.ToString() == id);

            if (album == null)
            {
                this.ViewBag["albumDetails"] = $"There are currently no album with {id}.";
            }
            else
            {
                var albumDetailsAsString = $"<img src={album.Cover} alt={album.Name} height= 200 width= 200 > </br>" +
                                           $"Name:{album.Name} </br>" +
                                           $"Price: {album.Price:f2}</br>" + 
                                           "<h3>Tracks</h3><hr/>" +
                                           $"<a href=\"/Tracks/Create?albumId={id}\">Create Track</a><hr/>";
                this.ViewBag["albumDetails"] = albumDetailsAsString;
                
                
                var albumTracksAsString = album.Tracks.Select(t=>$"<li><a href=\"/Tracks/Details?albumId={id}&trackId={t.Track.Id.ToString().ToUpper()}\"/>{t.Track.Name}</a></li>");
                
                this.ViewBag["albumTracks"] = album.Tracks.Count==0
                    ? "There are no tracks in album"
                    : $"<ol>{string.Join(Environment.NewLine,albumTracksAsString)}</ol>";

            }

            return this.View("Albums/Details");
        }


    }
}
