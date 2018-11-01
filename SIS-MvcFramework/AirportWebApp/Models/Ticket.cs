namespace AirportWebApp.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public virtual User Customer { get; set; }

        public int SeatId { get; set; }

        public virtual Seat Seat { get; set; }

        //public int FlightId { get; set; }

        //public virtual Flight Flight { get; set; }

        public int Quantity { get; set; } = 1;

        public bool IsPaid { get; set; } = false;
    }
}
