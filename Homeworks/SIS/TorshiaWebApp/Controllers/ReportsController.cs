namespace TorshiaWebApp.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Services.Contracts;
    using SIS.Framework.ActionResults;
    using SIS.Framework.Attributes.Actions;
    using SIS.Framework.Services.Contracts;
    using ViewModels;

    public class ReportsController : BaseController
    {
        public ReportsController(IUserCookieService userCookieService, ITaskService taskService, IReportService reportService, IUserService userService) : base(userCookieService)
        {
            this.TaskService = taskService;
            this.ReportService = reportService;
            this.UserService = userService;
        }

        public ITaskService TaskService { get; }
        public IReportService ReportService { get; }
        public IUserService UserService { get; }

        [Authorize("Admin")]
        public IActionResult Details(int id)
        {
            var report = this.ReportService.GetReportById(id);

            if (report == null)
            {
                this.Model.Data["Error"] = "There is no report with that id";
                return this.RedirectToAction("/");
            }

            var affectedSectorsAsString = new List<string>();
            foreach (var taskAffectedSector in report.Task.AffectedSectors)
            {
                var sector = taskAffectedSector.Sector.ToString();
                affectedSectorsAsString.Add(sector);
            }

            var reportAsViewModel = new ReportDetailsViewModel()
            {
                Id = report.Id,
                TaskAffectedSectors = string.Join(", ", affectedSectorsAsString),
                TaskDescription = report.Task.Description,
                TaskDueDate = report.Task.DueDate.ToString(),
                TaskTitle = report.Task.Title,
                TaskLevel = report.Task.AffectedSectors.Count(),
                TaskParticipants = report.Task.Participants,
                Status = report.Status.ToString(),
                ReportedOn = report.ReportedOn.ToString(),
                Username = report.Reporter.Username
            };

            this.Model.Data["TaskInfo"] = reportAsViewModel;

            return this.View();
        }

        [Authorize("Admin")]
        public IActionResult All()
        {
            var reports = this.ReportService.GetAllReports().ToList();

            var reportsAsViewModel = new List<ReportViewModel>();
            for (int i = 0; i < reports.Count(); i++)
            {
                reportsAsViewModel.Add(new ReportViewModel()
                {
                    Id = reports[i].Id,
                    Index = i + 1,
                    Level = reports[i].Task.AffectedSectors.Count(),
                    Status = reports[i].Status.ToString(),
                    Title = reports[i].Task.Title

                });
            }

            this.Model.Data["AllReports"] = reportsAsViewModel;

            return this.View();
        }

    }
}
