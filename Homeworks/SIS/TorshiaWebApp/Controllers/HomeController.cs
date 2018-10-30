namespace TorshiaWebApp.Controllers
{
    using Services.Contracts;
    using SIS.Framework.ActionResults;
    using SIS.Framework.Attributes.Methods;
    using SIS.Framework.Services.Contracts;
    using ViewModels;

    public class HomeController : BaseController
    {
        public ITaskService TaskService { get; }

        public HomeController(IUserCookieService userCookieService, ITaskService taskService) : base(userCookieService)
        {
            this.TaskService = taskService;
        }

        [HttpGet]
        public IActionResult Index(HomeIndexViewModel model)
        {
            if (!this.IsAuthenticated()) return this.View();

            this.Model.Data["Username"] = this.Identity.Username;
            this.Model.Data["AllNotReportedTasks"] = this.TaskService.GetAllNonReportedTasksAsModelWrappers();

            return this.View("Welcome");

        }
    }
}
