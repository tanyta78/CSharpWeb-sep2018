namespace IRunesWebApp.Controllers
{
    using Models;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;
    using System;
    using System.Linq;
    using System.Net;

    public class TrackController : BaseController
    {

        public IHttpResponse Create(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/Users/Login");
            }
            var albumId = request.QueryData["albumId"].ToString().ToUpper();
            this.ViewBag["backToAlbum"] = $"<a href=\"/Albums/Details?id={albumId}\">Back To Album</a>";
            this.ViewBag["formAction"] = $"action=\"/Tracks/Create?albumId={albumId}\"";
            return this.View("Tracks/Create");
        }

        public IHttpResponse DoCreate(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/Users/Login");
            }
            var name = request.FormData["name"].ToString().Trim();
            var link = WebUtility.UrlDecode(request.FormData["link"].ToString()).Replace("watch?v=","embed/");
           // src="https://www.youtube.com/watch?v=_avb2ikX-rQ<iframe width="640" height="480" src="https://www.youtube.com/embed/_avb2ikX-rQ" frameborder="0" allow="autoplay; encrypted-media" allowfullscreen></iframe>"
            var price = decimal.Parse(request.FormData["price"].ToString());
            var albumId = request.QueryData["albumId"].ToString().ToUpper();
            this.ViewBag["backToAlbum"] = $"<a href=\"/Albums/Details?id={albumId}\">Back To Album</a>";
            this.ViewBag["albumId"] = albumId;
            var track = new Track
            {
                Name = name,
                Link = link,
                Price = price
            };

            this.Db.Tracks.Add(track);
            var trackId = track.Id;
            var albumIdGuid = this.Db.Albums.FirstOrDefault(a => a.Id.ToString() == albumId).Id;
            var albumTrack = new AlbumTrack
            {
                AlbumId = albumIdGuid,
                TrackId = trackId
            };
            this.Db.AlbumsTracks.Add(albumTrack);
            try
            {

                
                this.Db.SaveChanges();
            }
            catch (Exception e)
            {
                //TODO: log error
                return this.ServerError(e.Message);
            }

            var response = this.Create(request);
            return response;
        }

        public IHttpResponse Details(IHttpRequest request)
        {

            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/Users/Login");
            }
         

            var albumId = request.QueryData["albumId"].ToString().ToUpper();
            var trackId = request.QueryData["trackId"].ToString().ToUpper();

            var track = this.Db.Tracks.FirstOrDefault(t => t.Id.ToString() == trackId);

            if (track == null)
            {
                this.ViewBag["trackDetails"] = $"Something went wrong. There are currently no track with {trackId}.";
            }
            else
            {
                var trackDetailsAsString = $"<iframe height=\"200\" width=\"200\"src=\"{track.Link}\" name=\"{track.Name}\"></iframe><p>Name: {track.Name}</p><br/><p>Price: ${track.Price}</p>";
                this.ViewBag["trackDetails"] = trackDetailsAsString;
            }

            this.ViewBag["backToAlbum"] = $"<a href=\"/Albums/Details?id={albumId}\">Back To Album</a>";

            return this.View("Tracks/Details");
        }


    }
}
