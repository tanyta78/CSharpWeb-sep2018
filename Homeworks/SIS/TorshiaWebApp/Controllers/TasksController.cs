namespace TorshiaWebApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models.Enums;
    using Services.Contracts;
    using SIS.Framework.ActionResults;
    using SIS.Framework.Attributes.Actions;
    using SIS.Framework.Attributes.Methods;
    using SIS.Framework.Services.Contracts;
    using ViewModels;

    public class TasksController : BaseController
    {
        public TasksController(IUserCookieService userCookieService, ITaskService taskService, IReportService reportService, IUserService userService) : base(userCookieService)
        {
            this.TaskService = taskService;
            this.ReportService = reportService;
            this.UserService = userService;
        }

        public ITaskService TaskService { get; }
        public IReportService ReportService { get; }
        public IUserService UserService { get; }

        [Authorize]
        public IActionResult Create()
        {
            return this.View();
        }


        [Authorize]
        [HttpPost]
        public IActionResult Create(CreateTaskInputModel model)
        {

            if (!this.ModelState.IsValid.HasValue || !this.ModelState.IsValid.Value)
            {
                this.Model.Data["Error"] = "Invalid input. Please fill all inputs.";
                return this.RedirectToAction("/Tasks/Create");
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
                this.Model.Data["Error"] = "Something went wrong when trying to create album in database.";
                return this.RedirectToAction("/Tasks/Create");
            }

            //TODO: add success message handle in viewEngine!!!
            this.Model.Data["Success"] = "Successfully create album in database.";
            return this.RedirectToAction("/Home/Index");
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var task = this.TaskService.GetTaskById(id);

            if (task == null)
            {
                this.Model.Data["Error"] = "There is no task with that id";
                return this.RedirectToAction("/");
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
                Participants = task.Participants
            };

            this.Model.Data["TaskInfo"] = taskAsViewModel;

            return this.View();
        }

        [Authorize]
        public IActionResult Report(int id)
        {
            var task = this.TaskService.ReportTaskById(id);

            if (task == null)
            {
                this.Model.Data["Error"] = "There is no task with that id";
                return this.RedirectToAction("/");
            }

            var user = this.UserService.GetUserByUsername(this.Identity.Username);
            
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
                this.Model.Data["Error"] = "A problem occured during reporting task. Please try again or contact admin.";
                return this.RedirectToAction("/");
            }

            return this.RedirectToAction("/");
        }

    }
}
