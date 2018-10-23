namespace IRunesWebApp.ViewModels
{
    using System.Collections.Generic;
    using Models;

    public class CreateTrackViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }

        public string Price { get; set; }

        public  ICollection<AlbumTrack> Albums { get; set; } 
    }
}