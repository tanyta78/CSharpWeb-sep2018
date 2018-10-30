namespace TorshiaWebApp.ViewModels
{
    using System.Collections.Generic;

    public class TaskViewModelWrapper
    {
        public ICollection<TaskViewModel> TaskViewModels { get; set; }=new List<TaskViewModel>();
    }
}