namespace CakesWebApp.Controllers
{
    using Models;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework;
    using System.Linq;
    using ViewModels.Cake;
    using ViewModels.Order;

    public class OrderController : BaseController
    {
        //Add to order
        [HttpPost("/orders/add")]
        public IHttpResponse Add(int productId)
        {
            var userId = this.Db.Users.FirstOrDefault(u => u.Username == this.User)?.Id;
            if (userId == null)
            {
                return this.BadRequestError("Please login first.");
            }
            var lastUserOrder = this.Db.Orders
                                    .Where(o => o.UserId == userId)
                                    .OrderByDescending(o => o.Id)
                                    .FirstOrDefault();

            if (lastUserOrder == null)
            {
                lastUserOrder = new Order
                {
                    UserId = userId.Value,
                };
                this.Db.Orders.Add(lastUserOrder);
                this.Db.SaveChanges();
            }

            var orderProduct = new OrderProduct
            {
                OrderId = lastUserOrder.Id,
                ProductId = productId
            };
            this.Db.OrderProducts.Add(orderProduct);
            this.Db.SaveChanges();

            return this.Redirect("/orders/byId?id=" + lastUserOrder.Id);

        }
        //Order by Id
        [HttpGet("/orders/byId")]
        public IHttpResponse GetOrderById(int id)
        {
            //order must exist and must be to current user
            var order = this.Db.Orders
                            .FirstOrDefault(o => o.Id == id && o.User.Username == this.User);

            if (order == null)
            {
                return this.BadRequestError($"Order with id {id} do not exist for current user.");
            }

            var products = this.Db.OrderProducts
                               .Where(po => po.OrderId == order.Id)
                               .Select(op => new CakeDetailsViewModel
                               {
                                   Id = op.Id,
                                   ImageUrl = op.Product.ImageUrl,
                                   Price = op.Product.Price,
                                   Name = op.Product.Name
                               })
                               .ToList();

            var lastOrderIdForUser = this.Db.Orders
                                         .Where(o => o.User.Username == this.User)
                                         .OrderByDescending(o => o.Id)
                                         .Select(x=>x.Id)
                                         .FirstOrDefault();

            var viewModel = new GetOrderByIdViewModel
            {
                Id = id,
                Products = products,
                IsShoppingCart = lastOrderIdForUser==order.Id
            };

            return this.View("GetOrderById", viewModel);
        }

        //List of order

        //Finish order (shopping cart=>order)
    }
}
