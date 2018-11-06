namespace PandaWebApp.ViewModels.Receipts
{
    using System.Collections.Generic;

    public class ReceiptsIndexViewModel
    {
        public ICollection<ReceiptViewModel> Receipts { get; set; } = new List<ReceiptViewModel>();
    }
}
