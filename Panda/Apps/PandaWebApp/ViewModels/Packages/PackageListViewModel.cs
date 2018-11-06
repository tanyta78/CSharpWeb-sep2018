namespace PandaWebApp.ViewModels.Packages
{
    using System.Collections.Generic;

    public class PackageListViewModel
    {
        public ICollection<PackageDetailViewModel> Packages { get; set; }
    }
}