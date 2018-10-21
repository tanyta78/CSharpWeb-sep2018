namespace CakesWebApp.ViewModels.Order
{
    using System.Collections.Generic;
    using Cake;

    public class GetOrderByIdViewModel
    {
        public int Id { get; set; }

        public bool IsShoppingCart { get; set; }

        public IEnumerable<CakeDetailsViewModel> Products { get; set; }
    }
}
