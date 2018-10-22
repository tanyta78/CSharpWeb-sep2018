namespace CakesWebApp.Controllers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Models;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Logger;
    using ViewModels.Cake;

    public class CakeController : BaseController
    {
        private readonly ILogger logger;

        public CakeController(ILogger logger)
        {
            this.logger = logger;
        }

        [HttpGet("/cakes/add")]
        public IHttpResponse AddCake()
        {
            return this.View("AddCake");
        }

        [HttpPost("/cakes/add")]
        public IHttpResponse DoAddCake(DoAddCakesInputModel model)
        {

            // TODO: VALIDATE INPUT 
           
            //CREATE PRODUCT
            //with object mapper
            var product = model.To<Product>();

            //without object mapper
            //var product = new Product
            //{
            //    Name = model.Name,
            //    Price = model.Price.ToDecimalOrDefault(),
            //    ImageUrl = model.ImageUrl
            //};

            this.Db.Products.Add(product);
            try
            {
                this.Db.SaveChanges();
            }
            catch (Exception e)
            {
                //TODO: log error
                return this.ServerError(e.Message);
            }


            //REDIRECT TO PRODUCT DETAILS PAGE
            return this.Redirect("/cakes/details?id="+ product.Id);
        }

        [HttpGet("/cakes/details")]
        public IHttpResponse Details()
        {
            var id = int.Parse(this.Request.QueryData["id"].ToString());
            var product = this.Db.Products.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return this.BadRequestError("Cake not found.");
            }

            //code for object mapper
            var viewModel = product.To<CakeDetailsViewModel>();

            //code without object mapper
            //var viewModel = new CakeDetailsViewModel()
            //{
            //    Name = product.Name,
            //    Price = decimal.Parse(product.Price.ToString(CultureInfo.InvariantCulture)),
            //    ImageUrl = product.ImageUrl,
            //    Id = id
            //};

            return this.View("CakeDetails", viewModel);
        }

        //cakes/search?searchText=cake
        [HttpGet("/cakes/search")]
        public IHttpResponse Search(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                searchText = "";
            }
            var cakes = this.Db.Products.Where(x => x.Name.Contains(searchText)).Select(p => new CakeDetailsViewModel
            {
                Name = p.Name,
                ImageUrl = p.ImageUrl,
                Price = p.Price,
                Id = p.Id
            }).ToArray();

            var model = new SearchViewModel
            {
                Cakes = cakes,
                SearchText = searchText
            };

            return this.View("CakeSearch", model);
        }
    }
}
