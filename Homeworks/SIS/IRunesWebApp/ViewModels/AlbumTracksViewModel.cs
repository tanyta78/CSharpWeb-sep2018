namespace IRunesWebApp.ViewModels
{
    using Models;

    public class AlbumTracksViewModel
    {
        public string AlbumId { get; set; }

        public Album Album { get; set; }

        public string TrackId { get; set; }

        public Track Track { get; set; }

        public string NameOfTrack { get; set; }
    }
}