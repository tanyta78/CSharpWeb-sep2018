namespace IssueTrackerWebApp.Controllers
{
    using SIS.HTTP.Responses;

    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            //if (this.User.IsLoggedIn)
            //{
            //    var products = this.Db.Products.Select(
            //        x => new ProductViewModel
            //        {
            //            Id = x.Id,
            //            Name = x.Name,
            //            Price = x.Price,
            //            Description = x.Description,
            //        }).ToList();
            //    var model = new IndexViewModel
            //    {
            //        Products = products,
            //    };

            //    return this.View("Home/IndexLoggedIn", model);
            //}

            return this.View();
        }
    }
}
