namespace CakesWebApp.Controllers
{
    using Models;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
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
            //if (string.IsNullOrWhiteSpace(username) || username.Length < 4)
            //{
            //    return this.BadRequestError("Please provide valid user name with length of 4 or more characters");
            //}

            //if (this.Db.Users.Any(x => x.Username == username))
            //{
            //    return this.BadRequestError("User with the same name already exist!");
            //}

            //if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            //{
            //    return this.BadRequestError("Please provide password  with length of 6 or more characters");
            //}

            //if (password != confirmPassword)
            //{
            //    return this.BadRequestError("Passwords do not match!");
            //}


            //CREATE PRODUCT
            var product = new Product
            {
                Name = model.Name,
                Price = model.Price.ToDecimalOrDefault(),
                ImageUrl = model.ImageUrl
            };

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


            //REDIRECT TO HOME PAGE
            return this.Redirect("/");
        }

        [HttpGet("/cakes/details")]
        public IHttpResponse Details()
        {
            var id = int.Parse(this.Request.QueryData["id"].ToString());
            var product = Db.Products.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return this.BadRequestError("Cake not found.");
            }
            //TODO: view model
            var viewBag = new Dictionary<string, string>();
            viewBag.Add("Name", product.Name);
            viewBag.Add("Price", product.Price.ToString(CultureInfo.InvariantCulture));
            viewBag.Add("ImageUrl", product.ImageUrl);
            return this.View("CakeDetails", viewBag);
        }
    }
}
