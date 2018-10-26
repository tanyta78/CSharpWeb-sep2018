namespace IRunesWebApp.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AlbumDetailsViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Cover { get; set; }

        public decimal Price => Math.Round(this.AlbumTracks.Sum(t => t.Track.Price) * 0.87m, 2);

        // public ICollection<UserAlbum> AlbumUsers { get; set; }

        public ICollection<AlbumTracksViewModel> AlbumTracks { get; set; }

        public string NoTracks { get; set; }

    }
}