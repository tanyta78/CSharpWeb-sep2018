namespace IRunesWebApp.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Models;

    public class AlbumDetailsViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Cover { get; set; }

        public decimal Price => this.AlbumTracks.Sum(t => t.Track.Price) * 0.87m;

        // public ICollection<UserAlbum> AlbumUsers { get; set; }

        public ICollection<AlbumTrack> AlbumTracks { get; set; }

    }
}