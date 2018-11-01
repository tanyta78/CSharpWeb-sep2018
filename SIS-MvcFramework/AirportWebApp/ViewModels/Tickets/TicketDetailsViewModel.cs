namespace AirportWebApp.ViewModels.Tickets
{
    using System;
    using Models;

    public class TicketDetailsViewModel
    {
        public int Id { get; set; }

        public string Destination { get; set; }

        public string Origin { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }
        
        public decimal Subtotal => this.Quantity * this.Price;

        public FlightClass Class { get; set; }
    }
}