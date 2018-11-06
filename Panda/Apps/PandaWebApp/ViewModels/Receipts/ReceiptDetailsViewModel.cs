namespace PandaWebApp.ViewModels.Receipts
{
    public class ReceiptDetailsViewModel
    {
        public int Id { get; set; }

        public decimal Fee { get; set; }

        public string IssuedOn { get; set; }

        public string Recipient { get; set; }

        public string Description { get; set; }

        public string Weight { get; set; }

        public string Address { get; set; }



    }
}