namespace AirportWebApp.ViewModels.Users
{
    using System.Collections.Generic;
    using Tickets;

    public class ProfileViewModel
    {
        public IEnumerable<TicketDetailsViewModel> MyFlightTicketDetails { get; set; }
    }
}