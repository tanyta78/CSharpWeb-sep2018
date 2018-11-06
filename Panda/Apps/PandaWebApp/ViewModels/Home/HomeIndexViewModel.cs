namespace PandaWebApp.ViewModels.Home
{
    using System.Collections.Generic;
    using Packages;

    public class HomeIndexViewModel
    {
        public ICollection<PackageDetailViewModel> Pending { get; set; }
        public ICollection<PackageDetailViewModel> Shipped { get; set; }
        public ICollection<PackageDetailViewModel> Delivered { get; set; }

    }
}
