namespace CakesWebApp.Controllers
{
    using Extensions;
    using Models;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class CakeController : BaseController
    {
        [HttpGet("/cakes/add")]
        public IHttpResponse AddCake()
        {
            return this.View("AddCake");
        }

        [HttpPost("/cakes/add")]
        public IHttpResponse DoAddCake()
        {
            var name = this.Request.FormData["name"].ToString().Trim().UrlDecode();
            var price = decimal.Parse(this.Request.FormData["price"].ToString().UrlDecode());
            var imageUrl = this.Request.FormData["imageUrl"].ToString().Trim().UrlDecode();

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
                Name = name,
                Price = price,
                ImageUrl = imageUrl
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

            var viewBag = new Dictionary<string, string>();
            viewBag.Add("Name", product.Name);
            viewBag.Add("Price", product.Price.ToString(CultureInfo.InvariantCulture));
            viewBag.Add("ImageUrl", product.ImageUrl);
            return this.View("CakeDetails", viewBag);
        }
    }
}
