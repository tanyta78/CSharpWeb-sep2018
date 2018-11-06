namespace PandaWebApp.Controllers
{
    using System.Linq;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;
    using ViewModels.Receipts;

    public class ReceiptsController : BaseController
    {
        [Authorize()]
        public IHttpResponse Index()
        {
            var receipts = this.Db.Receipts.Where(r => r.RecipientId == int.Parse(this.User.Info)).ToList().Select(r => new ReceiptViewModel()
            {
                Id = r.Id,
                Fee = r.Fee,
                IssuedOn = r.IssuedOn.ToShortDateString(),
                Recipient = this.User.Username
            }).ToList();
               

            var model = new ReceiptsIndexViewModel()
            {
                Receipts = receipts
            };

            return this.View(model);
        }

        [Authorize()]
        public IHttpResponse Details(int id)
        {
            var receipt = this.Db.Receipts.FirstOrDefault(r => r.Id == id);

            if (receipt == null)
            {
                return this.BadRequestError("Receipt with id do not exist.");
            }

            if (receipt.RecipientId.ToString() != this.User.Info)
            {
                return this.BadRequestError("You do not have rights to view this detail view.");
            }


            var model = new ReceiptDetailsViewModel()
            {
                Id = receipt.Id,
                Address = receipt.Package.ShippingAddress,
                Description = receipt.Package.Description,
                Fee = receipt.Fee,
                IssuedOn = receipt.IssuedOn.ToShortDateString(),
                Recipient = receipt.Recipient.Username,
                Weight = receipt.Package.Weight.ToString()
            };

            return this.View(model);
        }
    }
}
