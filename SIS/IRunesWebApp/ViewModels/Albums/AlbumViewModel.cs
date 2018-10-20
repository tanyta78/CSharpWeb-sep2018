using IRunesWebApp.Models;
using System.Collections.Generic;

namespace IRunesWebApp.ViewModels.Albums
{
    public class AlbumViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Cover { get; set; }

        public decimal Price { get; set; }

        public List<AlbumTrack> Tracks { get; set; }

        public int Count { get; set; }
    }
}