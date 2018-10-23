namespace IRunesWebApp.Controllers
{
    using System.Net;
    using Models;
    using Services.Contracts;
    using SIS.Framework.ActionResults;
    using SIS.Framework.Attributes.Methods;
    using SIS.Framework.Services.Contracts;
    using ViewModels;

    public class TracksController : BaseController
    {
        public ITrackService TrackService { get; }

        public TracksController(ITrackService trackService, IUserCookieService userCookieService) : base(userCookieService)
        {
            this.TrackService = trackService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!this.IsAuthenticated())
            {
                return this.RedirectToAction("/Users/Login");
            }

            var albumId = this.Request.QueryData["albumId"].ToString().ToUpper();
            this.Model.Data["backToAlbum"] = $"<a href=\"/Albums/Details?id={albumId}\" class=\"btn btn-success\" >Back To Album</a>";
            this.Model.Data["formAction"] = $"action=\"/Tracks/Create?albumId={albumId}\"";
            this.Model.Data["hrefAlbum"] = $"/Albums/Details?id={albumId}";

            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateTrackViewModel model)
        {
            if (!this.IsAuthenticated())
            {
                return this.RedirectToAction("/Users/Login");
            }

            if (!this.ModelState.IsValid.HasValue || !this.ModelState.IsValid.Value)
            {
                return this.RedirectToAction("/Tracks/Create");
            }

            var name = model.Name.Trim();
            var link = WebUtility.UrlDecode(model.Link).Replace("watch?v=", "embed/");

            // src="https://www.youtube.com/watch?v=_avb2ikX-rQ<iframe width="640" height="480" src="https://www.youtube.com/embed/_avb2ikX-rQ" frameborder="0" allow="autoplay; encrypted-media" allowfullscreen></iframe>"
            var price = decimal.Parse(model.Price);
            var albumId = this.Request.QueryData["albumId"].ToString().ToUpper();

            var track = new Track
            {
                Name = name,
                Link = link,
                Price = price
            };

            if (!this.TrackService.CreateTrack(track, albumId))
            {
                this.Model.Data["Error"] = "Something went wrong when trying to create track in database.";
                return this.RedirectToAction("/Tracks/Create");
            }

            //TODO: add success message handle in viewEngine!!!
            this.Model.Data["Success"] = $"Successfully create track in database for album with id {albumId}.";
            return this.RedirectToAction($"/Albums/Details?id={albumId}");

        }

        [HttpGet]
        public IActionResult Details()
        {

            if (!this.IsAuthenticated())
            {
                return this.RedirectToAction("/Users/Login");
            }


            var albumId = this.Request.QueryData["albumId"].ToString().ToUpper();
            var trackId = this.Request.QueryData["trackId"].ToString().ToUpper();

            var track = this.TrackService.GetTrackById(trackId);

            if (track == null)
            {
                this.Model.Data["trackDetails"] = $"Something went wrong. There are currently no track with {trackId}.";
            }
            else
            {
                var trackDetailsAsString = $"<h4 class=\"text-center\">Track Name: {track.Name}</h4>" +
                    $"<h4 class=\"text-center\">Track Price: ${track.Price}</h4>" +
                    "<hr class=\"bg-success\" />" +
                    "<div class=\"row text-center\"><div class=\"col-12\">" +
                    $"<iframe  src=\"{track.Link}\" name=\"{track.Name}\"></iframe>" +
                    "</div></div>";
                this.Model.Data["trackDetails"] = trackDetailsAsString;
            }

            this.Model.Data["backToAlbum"] = $"<a href=\"/Albums/Details?id={albumId}\" class=\"btn btn-success text-center\">Back To Album</a>";

            return this.View();
        }



    }
}
