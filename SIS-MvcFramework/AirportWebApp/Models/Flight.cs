namespace AirportWebApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Flight
    {
        public int Id { get; set; }

        public string Destination { get; set; }

        public string Origin { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }

        public string ImageUrl { get; set; }

        public bool PublicFlag { get; set; } = false;

        //public virtual ICollection<Seat> FlightSeats { get; set; } = new HashSet<Seat>();

        public virtual ICollection<Ticket> BookedTickets { get; set; } = new List<Ticket>();

        public int PaidTickets
        {
            get { return this.BookedTickets.Count(t => t.IsPaid = true); }
        }

    }
}
