using System.Linq;
using System.Web.Mvc;

namespace FullCalendarDemo.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return this.View();
        }


        public JsonResult GetEvents()
        {

            using (Database1Entities1 dc = new Database1Entities1())
            {

                var events = dc.Events.ToList();

                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }

        [HttpPost]
        public JsonResult SaveEvent(Event e)
        {
            var status = false;
            using (Database1Entities1 dc = new Database1Entities1())
            {
                if (e.Id > 0)
                {
                    //Update the event
                    var v = dc.Events.Where(a => a.Id == e.Id).FirstOrDefault();
                    if (v != null)
                    {
                        v.Subject = e.Subject;
                        v.Start = e.Start;
                        v.End = e.End;
                        v.Description = e.Description;
                        v.IsFullDay = e.IsFullDay;
                        v.ThemeColor = e.ThemeColor;
                    }
                }
                else
                {
                    dc.Events.Add(e);
                }
                dc.SaveChanges();
                status = true;
            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
            using (Database1Entities1 dc = new Database1Entities1())
            {
                var v = dc.Events.Where(a => a.Id == eventID).FirstOrDefault();
                if (v != null)
                {
                    dc.Events.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }
    }
}