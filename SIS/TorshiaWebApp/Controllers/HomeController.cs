namespace TorshiaWebApp.Controllers
{
    using System.Linq;
    using Services;
    using SIS.HTTP.Responses.Contracts;
    using ViewModels.Home;
    using ViewModels.Tasks;


    public class HomeController : BaseController
    {
        public TaskService TaskService { get; }

        public HomeController(TaskService taskService)
        {
            this.TaskService = taskService;
        }


        public IHttpResponse Index()
        {
            var user = this.Db.Users.FirstOrDefault(u => u.Username == this.User.Username);

            if (user == null)
            {
                return this.View();
            }

            var tasks = this.TaskService.GetAllNonReportedTasks().Select(t => new TaskViewModel()
            {
                Id = t.Id,
                Level = t.AffectedSectors.Count(),
                Title = t.Title
            });

            var welcomeViewModel = new WelcomeViewModel()
            {
                Username = this.User.Username,
                UserRole = this.User.Role,
                NonReportedTasks = tasks
            };


            return this.View("Home/Welcome", welcomeViewModel);

        }
    }
}
