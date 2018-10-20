using System.Collections.Generic;
using IRunesWebApp.Models;

namespace IRunesWebApp.ViewModels.Albums
{
    public class AllAlbumsViewModel
    {
        public int Count { get; set; }

        public List<Album> AllAlbums { get; set; }
    }
}
