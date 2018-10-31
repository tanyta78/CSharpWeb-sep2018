namespace TorshiaWebApp.Services.Contracts
{
    using System.Collections.Generic;
    using Models;
    using ViewModels.Tasks;

    public interface ITaskService
    {
        Task GetTaskById(int id);

        Task CreateTask(CreateTaskInputModel model);

        IEnumerable<Task> GetAllNonReportedTasks();

        Task ReportTaskById(int id);
    }
}