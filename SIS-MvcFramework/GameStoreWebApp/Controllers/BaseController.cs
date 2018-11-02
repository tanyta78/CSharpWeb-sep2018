namespace GameStoreWebApp.Controllers
{
    using GameStoreWebApp.Data;
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
