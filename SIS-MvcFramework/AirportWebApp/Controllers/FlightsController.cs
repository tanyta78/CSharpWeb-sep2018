namespace AirportWebApp.Controllers
{
    using System.Linq;
    using Models;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;
    using ViewModels.Flights;

    public class FlightsController : BaseController
    {
        [Authorize("Admin")]
        public IHttpResponse Add()
        {
            return this.View();
        }

        [Authorize("Admin")]
        [HttpPost()]
        public IHttpResponse Add(FlightsAddViewModel model)
        {
            var flight = new Flight()
            {
                Destination = model.Destination,
                Origin = model.Origin,
                Date = model.Date,
                Time = model.Time,
                ImageUrl = model.ImageUrl.UrlDecode()
            };

            this.Db.Flights.Add(flight);
            this.Db.SaveChanges();

            return this.Redirect("/");
        }

        [Authorize]
        public IHttpResponse Details(int id)
        {
            var flight = this.Db.Flights.FirstOrDefault(f => f.Id == id);

            if (flight == null)
            {
                return this.BadRequestError("Flight do not exist.");
            }

            var availableFligthSeats = this.Db.Seats.Where(s => s.FlightId == flight.Id).ToList();

            var model = new FlightDetailsViewModel()
            {
                Id = flight.Id,
                Destination = flight.Destination,
                Origin = flight.Origin,
                Date = flight.Date,
                Time = flight.Time,
                ImageUrl = flight.ImageUrl.UrlDecode(),
                AvailableFlightSeats = availableFligthSeats
            };

            return this.View(model);
        }

        [Authorize("Admin")]
        public IHttpResponse Publish(int id)
        {
            var flight = this.Db.Flights.FirstOrDefault(f => f.Id == id);

            if (flight == null)
            {
                return this.BadRequestError("Flight do not exist.");
            }

            flight.PublicFlag = true;
            this.Db.SaveChanges();

            return this.Redirect("/");
        }

        [Authorize("Admin")]
        public IHttpResponse Edit(int id)
        {
            var flight = this.Db.Flights.FirstOrDefault(f => f.Id == id);

            if (flight == null)
            {
                return this.BadRequestError("Flight do not exist.");
            }

            var model = new FlightDetailsViewModel()
            {
                Id = id,
                Destination = flight.Destination,
                Origin = flight.Origin,
                Date = flight.Date,
                Time = flight.Time,
                ImageUrl = flight.ImageUrl,
                PublicFlag = flight.PublicFlag
            };

            return this.View(model);
        }

        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Edit(FlightDetailsViewModel model)
        {
            var flight = this.Db.Flights.FirstOrDefault(f => f.Id == model.Id);

            if (flight == null)
            {
                return this.BadRequestError("Flight do not exist.");
            }

            //TODO verification
            flight.Destination = model.Destination;
            flight.Origin = model.Origin;
            flight.Date = model.Date;
            flight.Time = model.Time;
            flight.ImageUrl = model.ImageUrl;
            flight.PublicFlag = model.PublicFlag;

            this.Db.SaveChanges();

            return this.Redirect("/Flights/Details?id=" + flight.Id);
        }
    }
}
