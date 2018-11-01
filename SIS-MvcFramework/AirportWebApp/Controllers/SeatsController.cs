namespace AirportWebApp.Controllers
{
    using System.Linq;
    using Models;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;
    using ViewModels.Seats;

    public class SeatsController : BaseController
    {
        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Create(SeatsCreateViewModel model)
        {
            var flight = this.Db.Flights.FirstOrDefault(f => f.Id == model.FlightId);


            if (flight == null)
            {
                return this.BadRequestError("Flight do not exist.");
            }

            var seat = new Seat()
            {
                Class = model.Class,
                Price = model.Price,
                FlightId = model.FlightId,
                Quantity = model.Quantity
            };

            this.Db.Seats.Add(seat);
            this.Db.SaveChanges();

            return this.Redirect("/Flights/Details?id=" + model.FlightId);
        }

        [Authorize("Admin")]
        public IHttpResponse Delete(int id)
        {
            var seat = this.Db.Seats.FirstOrDefault(x => x.Id == id);
            if (seat == null)
            {
                return this.Redirect("/");
            }

            var flightId = seat.FlightId;

            this.Db.Remove(seat);
            // seat.IsDeleted = true;
            this.Db.SaveChanges();

            return this.Redirect("/Flights/Details?id=" + flightId);
        }
    }
}
