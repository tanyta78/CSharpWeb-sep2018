namespace IRunesWebApp.ViewModels
{
    using System.Collections.Generic;
    using Models;

    public class CreateAlbumViewModel
    {
        public string Name { get; set; }

        public string Cover { get; set; }

        public ICollection<AlbumTrack> Tracks { get; set; }
    }
}
