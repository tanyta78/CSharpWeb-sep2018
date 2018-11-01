namespace AirportWebApp.ViewModels.Home
{
    using System;

    public class FlightViewModel
    {
        public int Id { get; set; }

        public string Destination { get; set; }

        public string Origin { get; set; }

        public DateTime Date { get; set; }

       public string ImageUrl { get; set; }


    }
}