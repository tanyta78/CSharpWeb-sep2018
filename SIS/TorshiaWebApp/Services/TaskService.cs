namespace TorshiaWebApp.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Data;
    using Models;
    using ViewModels.Tasks;

    public class TaskService : ITaskService
    {
        private readonly AppDbContext db;

        public TaskService(AppDbContext db)
        {
            this.db = db;
        }


        public Task CreateTask(CreateTaskInputModel model)
        {
            var task = new Task()
            {
                Title = model.Title,
                Description = model.Description,
                DueDate = model.DueDate,
                IsReported = false,
                Participants = model.Participants,
            };

            this.db.Tasks.Add(task);

            var affectedSectors = new List<TaskSector>();

            foreach (var modelAffectedSector in model.AffectedSectors)
            {
                var taskSector = new TaskSector()
                {
                    Sector = modelAffectedSector,
                    TaskId = task.Id
                };
                this.db.TaskSectors.Add(taskSector);
                affectedSectors.Add(taskSector);
            }

            try
            {
                this.db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }



            return task;
        }

        public Task GetTaskById(int id)
        {
            var task = this.db.Tasks.FirstOrDefault(t => t.Id == id);
            return task;
        }

        public IEnumerable<Task> GetAllNonReportedTasks()
        {
            return this.db.Tasks.Where(t => t.IsReported == false).ToList();
        }

        public Task ReportTaskById(int id)
        {
            var task = this.GetTaskById(id);

            if (task != null)
            {
                task.IsReported = true;
                this.db.SaveChanges();
            }

            return task;
        }
    }
}