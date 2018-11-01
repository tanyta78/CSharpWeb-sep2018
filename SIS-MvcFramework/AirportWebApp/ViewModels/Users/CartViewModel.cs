namespace AirportWebApp.ViewModels.Users
{
    using System.Collections.Generic;
    using Tickets;

    public class CartViewModel
    {
        public IEnumerable<TicketDetailsViewModel> BookedTicketDetails { get; set; }
    }
}