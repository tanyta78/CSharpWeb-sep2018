namespace IRunesWebApp.ViewModels
{
    using Models;

    public class AlbumTracksViewModel
    {
        public string AlbumId { get; set; }

        public virtual Album Album { get; set; }

        public string TrackId { get; set; }

        public virtual Track Track { get; set; }

        public virtual string Trackname => this.Track.Name;
    }
}
