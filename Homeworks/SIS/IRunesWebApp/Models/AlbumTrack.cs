namespace IRunesWebApp.Models
{
    using System;

    public class AlbumTrack:BaseModel<int>
    {
        public Guid AlbumId { get; set; }

        public virtual Album Album { get; set; }

        public Guid TrackId { get; set; }

        public virtual Track Track { get; set; }
    }
}
