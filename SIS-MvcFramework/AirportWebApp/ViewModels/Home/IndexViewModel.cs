namespace AirportWebApp.ViewModels.Home
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public IEnumerable<FlightViewModel> Flights { get; set; }
    }
}
