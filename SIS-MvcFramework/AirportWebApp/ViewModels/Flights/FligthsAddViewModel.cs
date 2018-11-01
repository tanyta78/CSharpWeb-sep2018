namespace AirportWebApp.Models
{
    using System;

    public class FlightsAddViewModel
    {
        public string Destination { get; set; }

        public string Origin { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }

        public string ImageUrl { get; set; }

    }
}
