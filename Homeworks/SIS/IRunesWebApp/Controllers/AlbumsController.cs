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
            var allAlbums = this.AlbumService.GetAllAlbums()
                .Select(a => new AlbumViewModel { Id = a.Id.ToString().ToUpper(), Name = a.Name });

            var allAlbumsViewModel = new AllAlbumsViewModel()
            {
                AllAlbums = allAlbums
            };


            this.Model.Data["allAlbums"] = allAlbumsViewModel;

            return this.View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CreateAlbumViewModel model)
        {

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
           var album = this.AlbumService.GetAlbumById(model.Id);

            if (album == null)
            {
                this.Model.Data["albumDetails"] = $"There are currently no album with {model.Id}.";
            }
            else
            {
                var albumTracksAsViewModel = album.Tracks.Select(at => new AlbumTracksViewModel
                {
                    Album = at.Album,
                    AlbumId = at.AlbumId.ToString().ToUpper(),
                    Track = at.Track,
                    TrackId = at.TrackId.ToString().ToUpper()
                }).ToList();

                foreach (var albumTracks in albumTracksAsViewModel)
                {
                    Console.WriteLine($"Album Name {albumTracks.Album.Name} Track Name {albumTracks.Track.Name} TRackName {albumTracks.Trackname}");
                }

                var albumsAsViewModel = new AlbumDetailsViewModel()
                {
                    Cover = album.Cover,
                    Id = album.Id.ToString().ToUpper(),
                    Name = album.Name,
                    AlbumTracks = albumTracksAsViewModel
                };


                if (album.Tracks.Count == 0)
                {
                    albumsAsViewModel.NoTracks = "There are no tracks in album";
                }

                this.Model.Data["albumDetails"] = albumsAsViewModel;


            }

            return this.View();
        }
    }
}
