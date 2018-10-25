namespace IRunesWebApp.Controllers
{
    using System;
    using System.Linq;
    using Services.Contracts;
    using SIS.Framework.ActionResults;
    using SIS.Framework.Attributes.Actions;
    using SIS.Framework.Attributes.Methods;
    using SIS.Framework.Services.Contracts;
    using ViewModels;

    public class AlbumsController : BaseController
    {
        public AlbumsController(IAlbumService albumService, IUserCookieService cookieService) : base(cookieService)
        {
            this.AlbumService = albumService;
        }

        public IAlbumService AlbumService { get; }

        [Authorize]
        [HttpGet]
        public IActionResult All()
        {
            //if (!this.IsAuthenticated())
            //{
            //    return this.RedirectToAction("/Users/Login");
            //}

            var allAlbums = this.AlbumService.GetAllAlbums();
            var allAlbumsString = String.Join(Environment.NewLine, allAlbums);

            this.Model.Data["allAlbums"] = string.IsNullOrWhiteSpace(allAlbumsString)
                ? "There are currently no albums."
                : allAlbumsString;

            return this.View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            //if (!this.IsAuthenticated())
            //{
            //    return this.RedirectToAction("/Users/Login");
            //}

            return this.View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CreateAlbumViewModel model)
        {
            //if (!this.IsAuthenticated())
            //{
            //    return this.RedirectToAction("/Users/Login");
            //}

            if (!this.ModelState.IsValid.HasValue || !this.ModelState.IsValid.Value)
            {
                return this.RedirectToAction("/Albums/Create");
            }

            if (this.AlbumService.CreateAlbum(model.Name, model.Cover) == null)
            {
                this.Model.Data["Error"] = "Something went wrong when trying to create album in database.";
                return this.RedirectToAction("/Albums/Create");
            }

            //TODO: add success message handle in viewEngine!!!
            this.Model.Data["Success"] = "Successfully create album in database.";
            return this.RedirectToAction("/Albums/All");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Details(AlbumDetailsViewModel model)
        {
            //if (!this.IsAuthenticated())
            //{
            //    return this.RedirectToAction("/Users/Login");
            //}

            //var id = this.Request.QueryData["id"].ToString().ToUpper();
            var album = this.AlbumService.GetAlbumById(model.Id);

            if (album == null)
            {
                this.Model.Data["albumDetails"] = $"There are currently no album with {model.Id}.";
            }
            else
            {
                var albumDetailsAsString = $"<img src={album.Cover} alt={album.Name} class=\"img-fluid\" > " +
                                           $"<h4 class=\"text-center\">Album Name:{album.Name} </h4>" +
                                           $"<h4 class=\"text-center\">Album Price:$ {album.Price:f2}</h4>";
                var createTrack = $"<a href=\"/Tracks/Create?albumId={model.Id}\" class=\"btn btn-success\">Create Track</a>";
                this.Model.Data["albumDetails"] = albumDetailsAsString;
                this.Model.Data["createTrack"] = createTrack;


                var albumTracksAsString = album.Tracks.Select(t => $"<li><a href=\"/Tracks/Details?albumId={model.Id}&trackId={t.Track.Id.ToString().ToUpper()}\"/>{t.Track.Name}</a></li>");

                this.Model.Data["albumTracks"] = album.Tracks.Count == 0
                    ? "There are no tracks in album"
                    : $"<ol>{string.Join(Environment.NewLine, albumTracksAsString)}</ol>";

            }

            return this.View();
        }
    }
}
