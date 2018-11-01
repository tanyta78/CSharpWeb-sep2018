namespace AirportWebApp.Controllers
{
    using System.Linq;
    using Models;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;
    using ViewModels.Tickets;

    public class TicketsController : BaseController
    {
        [Authorize]
        [HttpPost()]
        public IHttpResponse Add(TicketsAddViewModel model)
        {
            var flight = this.Db.Flights.FirstOrDefault(f => f.Id == model.FlightId);

            if (flight == null)
            {
                return this.BadRequestError("Flight do not exist.");
            }

            var seat = this.Db.Seats.FirstOrDefault(s => s.Id == model.SeatId);

            if (seat == null)
            {
                return this.BadRequestError("Seat do not exist.");
            }

            var user = this.Db.Users.FirstOrDefault(u => u.Email == this.User.Info);

            if (user == null)
            {
                return this.BadRequestError("User do not exist.");
            }

            var ticket = new Ticket()
            {
                CustomerId = user.Id,
                //FlightId = flight.Id,
                SeatId = seat.Id,
                Quantity = model.Quantity
            };

            //TODO check available flight seat quantity. If some ticket was paid =>change flight seat quantity with ticket quantity. This functionality is not implemented in requirements.

            this.Db.Tickets.Add(ticket);
            this.Db.SaveChanges();
            var ticketId = ticket.Id;
            return this.Redirect("/tickets/details?id=" + ticketId);
        }

        [Authorize()]
        public IHttpResponse Details(int id)
        {

            var ticket = this.Db.Tickets.FirstOrDefault(t => t.Id == id);

            if (ticket == null)
            {
                return this.BadRequestError("Ticket do not exist.");
            }

            var ticketDetailViewModel = new TicketDetailsViewModel
            {
                Id = id,
                Destination = ticket.Seat.Flight.Destination,
                Origin = ticket.Seat.Flight.Origin,
                Date = ticket.Seat.Flight.Date,
                Time = ticket.Seat.Flight.Time,
                Quantity = ticket.Quantity,
                Class = ticket.Seat.Class,
                Price = ticket.Seat.Price
            };

            return this.View(ticketDetailViewModel);
        }

        [Authorize()]
        public IHttpResponse Delete(int id)
        {

            var ticket = this.Db.Tickets.FirstOrDefault(t => t.Id == id);

            if (ticket == null)
            {
                return this.BadRequestError("Ticket do not exist.");
            }

            this.Db.Tickets.Remove(ticket);
            this.Db.SaveChanges();

            return this.Redirect("/");
        }

        [Authorize()]
        public IHttpResponse Checkout(int id)
        {
            var ticket = this.Db.Tickets.FirstOrDefault(t => t.Id == id);

            if (ticket == null)
            {
                return this.BadRequestError("Ticket do not exist.");
            }

            //check are available seats 
            var paidTickets = ticket.Seat.Flight.PaidTickets;
            var availableTicketsForClass = ticket.Seat.Quantity;
            if (availableTicketsForClass >= paidTickets + ticket.Quantity)
            {
                ticket.IsPaid = true;
            }

            this.Db.SaveChanges();
            return this.Redirect("/");
        }
    }
}
