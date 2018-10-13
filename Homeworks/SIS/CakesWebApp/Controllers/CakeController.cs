namespace CakesWebApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Extensions;
    using Models;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;

    public class CakeController:BaseController
    {
        public IHttpResponse AddCake(IHttpRequest request)
        {
            return this.View("AddCake");
        }

        public IHttpResponse DoAddCake(IHttpRequest request)
        {
            var name = request.FormData["name"].ToString().Trim().UrlDecode();
            var price = decimal.Parse(request.FormData["price"].ToString().UrlDecode());
            var imageUrl = request.FormData["imageUrl"].ToString().Trim().UrlDecode();

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
            return new RedirectResult("/");
        }

        public IHttpResponse Details(IHttpRequest request)
        {
            var id = int.Parse(request.QueryData["id"].ToString());
            var product = Db.Products.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return this.BadRequestError("Cake not found.");
            }

            var viewBag = new Dictionary<string, string>();
            viewBag.Add("Name",product.Name);
            viewBag.Add("Price",product.Price.ToString(CultureInfo.InvariantCulture));
            viewBag.Add("ImageUrl",product.ImageUrl);
            return this.View("CakeDetails",viewBag);
        }
    }
}
