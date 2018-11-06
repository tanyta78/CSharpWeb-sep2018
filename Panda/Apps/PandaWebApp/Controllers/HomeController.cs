namespace PandaWebApp.Controllers
{
    using System.Linq;
    using SIS.HTTP.Responses;
    using ViewModels.Home;
    using ViewModels.Packages;

    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            if (this.User.IsLoggedIn)
            {
                var packages = this.Db.Packages.ToList();
                if (this.User.Role!="Admin")
                {
                    packages = packages.Where(p => p.RecipientId == int.Parse(this.User.Info)).ToList();
                }


                var pending = packages.Where(p => p.Status.ToString() == "Pending").ToList().Select(p=>new PackageDetailViewModel()
                {
                    Description = p.Description,
                    Id = p.Id
                }).ToList();

                var shipped = packages.Where(p => p.Status.ToString() == "Shipped").ToList().Select(p=>new PackageDetailViewModel()
                {
                    Description = p.Description,
                    Id = p.Id
                }).ToList();

                var delivered = packages.Where(p => p.Status.ToString() == "Delivered").ToList().Select(p=>new PackageDetailViewModel()
                {
                    Description = p.Description,
                    Id = p.Id
                }).ToList();
               

                var model = new HomeIndexViewModel()
                {
                    Pending = pending,
                    Shipped = shipped,
                    Delivered = delivered
                };

                return this.View("Home/IndexLoggedIn", model);
            }

            return this.View();
        }
    }
}
