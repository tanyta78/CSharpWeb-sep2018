namespace PandaWebApp.Controllers
{
    using System;
    using System.Linq;
    using Models;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;
    using ViewModels.Packages;

    public class PackagesController : BaseController
    {
        [Authorize("Admin")]
        public IHttpResponse Create()
        {
            var users = Db.Users.Select(u => new RecipientViewModel()
            {
                Id = u.Id,
                Username = u.Username
            });

            var model = new PackageCreateInputModel()
            {
                Recipients = users.ToList()
            };

            return this.View(model);
        }

        [Authorize("Admin")]
        [HttpPost()]
        public IHttpResponse Create(PackageCreateInputModel model)
        {
            var package = new Package()
            {
                Description = model.Description,
                ShippingAddress = model.ShippingAddress,
                Weight = model.Weight,
                EstimateDeliveryDate = null,
                RecipientId = model.RecipientId,
                Status = Status.Pending
            };

            this.Db.Packages.Add(package);
            this.Db.SaveChanges();

            return this.Redirect("/");
        }

        [Authorize()]
        public IHttpResponse Details(int id)
        {
            var package = this.Db.Packages.FirstOrDefault(p => p.Id == id);

            if (package == null)
            {
                return this.BadRequestError("Package with id do not exist.");
            }

            if (package.RecipientId.ToString() != this.User.Info)
            {
                return this.BadRequestError("You do not have rights to view this detail view.");

            }

            var date = "N/A";

            if (package.Status == Status.Shipped)
            {
                date = package.EstimateDeliveryDate.ToString();
            }

            if (package.Status == Status.Delivered || package.Status == Status.Acquired)
            {
                date = "Delivered";
            }

            var model = new PackageDetailViewModel()
            {
                Id = package.Id,
                Description = package.Description,
                ShippingAddress = package.ShippingAddress,
                Status = package.Status.ToString(),
                EstimateDeliveryDate = date,
                Weight = package.Weight,
                Recipient = package.Recipient.Username
            };

            return this.View(model);
        }

        [Authorize("Admin")]
        public IHttpResponse Pending()
        {
            var packages = this.Db.Packages.Where(p => p.Status == Status.Pending).Select(package => new PackageDetailViewModel()
            {
                Id = package.Id,
                Description = package.Description,
                ShippingAddress = package.ShippingAddress,
                Weight = package.Weight,
                Recipient = package.Recipient.Username
            }).ToList();

            var model = new PackageListViewModel()
            {
                Packages = packages
            };

            return this.View(model);
        }

        [Authorize("Admin")]
        public IHttpResponse Shipped()
        {
            var packages = this.Db.Packages.Where(p => p.Status == Status.Shipped).Select(package => new PackageDetailViewModel()
            {
                Id = package.Id,
                Description = package.Description,
                EstimateDeliveryDate = package.EstimateDeliveryDate.ToString(),
                Weight = package.Weight,
                Recipient = package.Recipient.Username
            }).ToList();

            var model = new PackageListViewModel()
            {
                Packages = packages
            };

            return this.View(model);
        }

        [Authorize("Admin")]
        public IHttpResponse Delivered()
        {
            var packages = this.Db.Packages.Where(p => p.Status == Status.Delivered).Select(package => new PackageDetailViewModel()
            {
                Id = package.Id,
                Description = package.Description,
                ShippingAddress = package.ShippingAddress,
                Weight = package.Weight,
                Recipient = package.Recipient.Username
            }).ToList();

            var model = new PackageListViewModel()
            {
                Packages = packages
            };

            return this.View(model);
        }

        [Authorize("Admin")]
        public IHttpResponse Ship(int id)
        {
            var package = this.Db.Packages.FirstOrDefault(p => p.Id == id);

            if (package == null)
            {
                return this.BadRequestError("Package with id do not exist.");
            }

            var rnd = new Random();
            var date = DateTime.Now.AddDays(rnd.Next(20, 40));
            package.Status = Status.Shipped;
            package.EstimateDeliveryDate = date;
            this.Db.SaveChanges();
            return this.Redirect("/");
        }

        [Authorize("Admin")]
        public IHttpResponse Deliver(int id)
        {
            var package = this.Db.Packages.FirstOrDefault(p => p.Id == id);

            if (package == null)
            {
                return this.BadRequestError("Package with id do not exist.");
            }

            package.Status = Status.Delivered;
            this.Db.SaveChanges();
            return this.Redirect("/");
        }

        [Authorize()]
        public IHttpResponse Acquire(int id)
        {
            var package = this.Db.Packages.FirstOrDefault(p => p.Id == id);

            if (package == null)
            {
                return this.BadRequestError("Package with id do not exist.");
            }

            if (package.RecipientId.ToString() != this.User.Info)
            {
                return this.BadRequestError("You do not have rights to view this detail view.");

            }

            package.Status = Status.Acquired;

           
            var receipt = new Receipt()
            {
                PackageId = package.Id,
                RecipientId = int.Parse(this.User.Info),
                Fee = package.Weight*2.67m,
                IssuedOn = DateTime.Now
            };
            this.Db.Receipts.Add(receipt);
            this.Db.SaveChanges();
            return this.Redirect("/");
        }
    }
}
