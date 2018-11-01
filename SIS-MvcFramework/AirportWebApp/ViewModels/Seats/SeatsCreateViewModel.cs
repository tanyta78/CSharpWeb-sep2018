namespace AirportWebApp.ViewModels.Seats
{
    using Models;

    public class SeatsCreateViewModel
    {
        public FlightClass Class { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int FlightId { get; set; }

    }
}