namespace PandaWebApp.ViewModels.Packages
{
    public class PackageDetailViewModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public decimal Weight { get; set; }

        public string ShippingAddress { get; set; }

        public string Status { get; set; }

        public string EstimateDeliveryDate { get; set; }

        public string Recipient { get; set; }

    }
}
