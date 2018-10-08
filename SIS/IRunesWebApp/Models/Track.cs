namespace IRunesWebApp.Models
{
    using System;
    using System.Collections.Generic;

    public class Track : BaseModel<Guid>
    {
        public string Name { get; set; }

        public string Link { get; set; }

        public decimal Price { get; set; }

        public virtual ICollection<AlbumTrack> Albums { get; set; } = new HashSet<AlbumTrack>();
    }
}
