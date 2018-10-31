namespace TorshiaWebApp.ViewModels.Home
{
    using System.Collections.Generic;
    using Tasks;

    public class WelcomeViewModel
    {
        public string Username { get; set; }
        public string UserRole { get; set; }
        public IEnumerable<TaskViewModel> NonReportedTasks { get; set; } = new List<TaskViewModel>();
    }
}
