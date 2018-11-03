namespace IssueTrackerWebApp.Controllers
{
    using Data;
    using SIS.MvcFramework;

    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
            this.Db = new AppDbContext();
        }

        public AppDbContext Db { get; }
    }
}
