namespace AirportWebApp.ViewModels.Flights
{
    using System;
    using System.Collections.Generic;
    using Models;

    public class FlightDetailsViewModel
    {
        public int Id { get; set; }

        public string Destination { get; set; }

        public string Origin { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }

        public string ImageUrl { get; set; }

        public bool PublicFlag { get; set; } 
        
        public virtual ICollection<Seat> AvailableFlightSeats { get; set; } = new HashSet<Seat>();

        public virtual ICollection<Ticket> BoughtTickets { get; set; } = new List<Ticket>();

    }
}