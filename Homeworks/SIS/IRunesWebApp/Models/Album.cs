namespace IRunesWebApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public class Album : BaseModel<Guid>
    {
        public string Name { get; set; }

        public string Cover { get; set; }

        //•	Price – a decimal (sum of all Tracks’ prices, reduced by 13%).
        [NotMapped]
        public decimal? Price => this.Tracks.Sum(t => t.Track.Price) * 0.87m;

        //•	Tracks – a collection of Tracks.
        public virtual ICollection<AlbumTrack> Tracks { get; set; } = new HashSet<AlbumTrack>();
    }
}
