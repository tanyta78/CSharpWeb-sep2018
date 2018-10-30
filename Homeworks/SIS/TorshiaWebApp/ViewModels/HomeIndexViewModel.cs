namespace TorshiaWebApp.ViewModels
{
    using System.Collections.Generic;

    public class HomeIndexViewModel
    {
        public ICollection<TaskViewModelWrapper> AllNotReportedTasks { get; set; } = new List<TaskViewModelWrapper>();
    }
}