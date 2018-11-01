namespace AirportWebApp.Controllers
{
    using System.Linq;
    using Models;
    using SIS.HTTP.Responses;
    using ViewModels.Home;

    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            var flights = this.Db.Flights.Where(f => f.PublicFlag == true).ToList();


            if (this.User.Role == UserRole.Admin.ToString())
            {
                flights = this.Db.Flights.ToList();
            }

            var flightsAsViewModel = flights.Select(
                x => new FlightViewModel
                {
                    Id = x.Id,
                    Destination = x.Destination,
                    Date = x.Date,
                    ImageUrl = x.ImageUrl,
                    Origin = x.Origin
                });

            var model = new IndexViewModel
            {
                Flights = flightsAsViewModel,
            };

            return this.View(model);
        }
    }
}
