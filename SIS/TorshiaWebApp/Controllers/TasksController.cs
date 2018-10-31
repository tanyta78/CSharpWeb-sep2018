namespace TorshiaWebApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using Models.Enums;
    using Services;
    using Services.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework;
    using ViewModels.Tasks;

    public class TasksController : BaseController
    {
        public TasksController(TaskService taskService, ReportService reportService, UserService userService)
        {
            this.TaskService = taskService;
            this.ReportService = reportService;
            this.UserService = userService;
        }

        public TaskService TaskService { get; }
        public ReportService ReportService { get; }
        public UserService UserService { get; }

        [Authorize]
        public IHttpResponse Create()
        {
            if (this.User.Role != Role.Admin.ToString())
            {
                return this.BadRequestError("You do not have permission to access this functionality!");
            }

            var model = new TaskInfoViewModel()
            {
                UserRole = this.User.Role
            };
            return this.View(model);
        }


        [Authorize]
        [HttpPost]
        public IHttpResponse Create(CreateTaskInputModel model)
        {
            if (this.User.Role != Role.Admin.ToString())
            {
                return this.BadRequestError("You do not have permission to access this functionality!");
            }
            if (string.IsNullOrWhiteSpace(model.Title))
            {
                return this.BadRequestErrorWithView("Please enter title!");
            }
            if (string.IsNullOrWhiteSpace(model.Description))
            {
                return this.BadRequestErrorWithView("Please enter description!");
            }
            if (string.IsNullOrWhiteSpace(model.Participants))
            {
                return this.BadRequestErrorWithView("Please enter participants!");
            }

            model.AffectedSectors = new List<SectorType>();

            // to ugly but... no other way to do it easy
            if (model.CustomersSector != null)
            {
                System.Enum.TryParse(model.CustomersSector, out SectorType sectorType);
                model.AffectedSectors.Add(sectorType);
            }
            if (model.FinancesSector != null)
            {
                System.Enum.TryParse(model.FinancesSector, out SectorType sectorType);
                model.AffectedSectors.Add(sectorType);
            }
            if (model.InternalSector != null)
            {
                System.Enum.TryParse(model.InternalSector, out SectorType sectorType);
                model.AffectedSectors.Add(sectorType);
            }
            if (model.ManagementSector != null)
            {
                System.Enum.TryParse(model.ManagementSector, out SectorType sectorType);
                model.AffectedSectors.Add(sectorType);
            }
            if (model.MarketingSector != null)
            {
                System.Enum.TryParse(model.MarketingSector, out SectorType sectorType);
                model.AffectedSectors.Add(sectorType);
            }

            if (this.TaskService.CreateTask(model) == null)
            {
                return this.BadRequestErrorWithView("Something went wrong when trying to create album in database.");

            }

            return this.Redirect("/Home/Index");
        }

        [Authorize]
        public IHttpResponse Details(int id)
        {
           var task = this.TaskService.GetTaskById(id);

            if (task == null)
            {
                return this.BadRequestError("Invalid task id");
            }

            var affectedSectorsAsString = new List<string>();
            foreach (var taskAffectedSector in task.AffectedSectors)
            {
                var sector = taskAffectedSector.Sector.ToString();
                affectedSectorsAsString.Add(sector);
            }

            var taskAsViewModel = new TaskInfoViewModel()
            {
                Id = task.Id,
                AffectedSectors = string.Join(", ", affectedSectorsAsString),
                Description = task.Description,
                DueDate = task.DueDate,
                Title = task.Title,
                Level = task.AffectedSectors.Count(),
                Participants = task.Participants,
                UserRole = this.User.Role
            };

            return this.View(taskAsViewModel);
        }

        [Authorize]
        public IHttpResponse Report(int id)
        {
            var task = this.TaskService.ReportTaskById(id);

            if (task == null)
            {
                return this.BadRequestError("Invalid task id");
            }

            var user = this.UserService.GetUserByUsername(this.User.Username);

            var reportModel = new CreateReportInputModel()
            {
                ReportedOn = DateTime.Now,
                Reporter = user,
                Task = task
            };

            var rnd = new Random();
            var percent = rnd.Next(0, 100);
            reportModel.Status = percent > 25 ? Status.Completed : Status.Archived;

            var report = this.ReportService.CreateReport(reportModel);

            if (report == null)
            {
                return this.BadRequestError("A problem occured during reporting task. Please try again or contact admin.");
               }

            return this.Redirect("/");
        }

    }
}
