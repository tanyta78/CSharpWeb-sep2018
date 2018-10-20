namespace IRunesWebApp.Controllers
{
    using Models;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework;
    using SIS.WebServer.Results;
    using System;
    using System.Linq;
    using System.Net;
    using ViewModels.Tracks;

    public class TrackController : BaseController
    {
        [HttpGet("/Tracks/Create")]
        public IHttpResponse Create(IHttpRequest request)
        {
            if (!this.IsAuthenticated())
            {
                return this.Redirect("/Users/Login");
            }
            var albumId = request.QueryData["albumId"].ToString().ToUpper();
            var model = new DoCreateTrackInputModel
            {
                albumId = albumId
            };

            return this.View("Tracks/Create",model);
        }
        [HttpPost("/Tracks/Create")]
        public IHttpResponse DoCreate(DoCreateTrackInputModel model)
        {
            if (!this.IsAuthenticated())
            {
                return this.Redirect("/Users/Login");
            }

            var link = WebUtility.UrlDecode(model.Link).Replace("watch?v=", "embed/");
            // src="https://www.youtube.com/watch?v=_avb2ikX-rQ<iframe width="640" height="480" src="https://www.youtube.com/embed/_avb2ikX-rQ" frameborder="0" allow="autoplay; encrypted-media" allowfullscreen></iframe>"

            var albumId = this.Request.QueryData["albumId"].ToString().ToUpper();

            var track = new Track
            {
                Name = model.Name.Trim(),
                Link = link,
                Price = decimal.Parse(model.Price)
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

            return this.Create(this.Request);
        }

        [HttpGet("/Tracks/Details")]
        public IHttpResponse Details()
        {

            if (!this.IsAuthenticated())
            {
                return this.Redirect("/Users/Login");
            }


            var albumId = this.Request.QueryData["albumId"].ToString().ToUpper();
            var trackId = this.Request.QueryData["trackId"].ToString().ToUpper();

            var track = this.Db.Tracks.FirstOrDefault(t => t.Id.ToString() == trackId);
            var model = new DoCreateTrackInputModel();
            if (track == null)
            {
                return new BadRequestResult("Something went wrong. There are currently no track with {trackId}.");
            }
            else
            {
                model.albumId = albumId;
                model.Name = track.Name;
                model.Link = track.Link;
                model.Price = track.Price.ToString();
            }

            

            return this.View("Tracks/Details", model);
        }


    }
}
