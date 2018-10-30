namespace TorshiaWebApp.Services.Contracts
{
    using System.Collections.Generic;
    using Models;
    using ViewModels;

    public interface ITaskService
    {
        Task GetTaskById(int id);

        Task CreateTask(CreateTaskInputModel model);

        IEnumerable<Task> GetAllNonReportedTasks();

        IEnumerable<TaskViewModelWrapper> GetAllNonReportedTasksAsModelWrappers();

        Task ReportTaskById(int id);
    }
}