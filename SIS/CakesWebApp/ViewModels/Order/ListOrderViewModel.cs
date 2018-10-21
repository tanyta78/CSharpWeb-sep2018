namespace CakesWebApp.ViewModels.Order
{
    using System;

    public class ListOrderViewModel
    {

        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public int NumberOfProducts { get; set; }

        public decimal SumOfProductsPrice { get; set; }

        public bool IsFinished { get; set; }

    }
}