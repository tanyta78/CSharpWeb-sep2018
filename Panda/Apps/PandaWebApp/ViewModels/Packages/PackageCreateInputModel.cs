namespace PandaWebApp.ViewModels.Packages
{
    using System.Collections.Generic;

    public class PackageCreateInputModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public decimal Weight { get; set; }

        public string ShippingAddress { get; set; }

        public ICollection<RecipientViewModel> Recipients { get; set; }

        public int RecipientId { get; set; }


    }
}
