namespace AirportWebApp.Models
{
    public class Seat
    {
        public int Id { get; set; }

        public FlightClass Class { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int FlightId { get; set; }

        public virtual Flight Flight { get; set; }
    }
}
