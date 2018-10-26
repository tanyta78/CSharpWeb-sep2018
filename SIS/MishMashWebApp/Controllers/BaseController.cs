namespace MishMashWebApp.Controllers
{
    using Data;
    using SIS.MvcFramework;

    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
            this.Db = new AppDbContext();
        }

        protected AppDbContext Db { get; }


    }
}
