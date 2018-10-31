namespace TorshiaWebApp.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using Services;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework;
    using ViewModels;
    using ViewModels.Reports;

    public class ReportsController : BaseController
    {
        public ReportsController(TaskService taskService, ReportService reportService, UserService userService)
        {
            this.TaskService = taskService;
            this.ReportService = reportService;
            this.UserService = userService;
        }

        public TaskService TaskService { get; }
        public ReportService ReportService { get; }
        public UserService UserService { get; }

        [Authorize]
        public IHttpResponse Details(int id)
        {
            if (this.User.Role != Role.Admin.ToString())
            {
                return this.BadRequestError("You do not have permission to access this functionality!");
            }

            var report = this.ReportService.GetReportById(id);

            if (report == null)
            {
                return this.BadRequestError("Invalid report id");

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
                Username = report.Reporter.Username,
                UserRole = this.User.Role
            };


            return this.View(reportAsViewModel);
        }

        [Authorize]
        public IHttpResponse All()
        {
            if (this.User.Role != Role.Admin.ToString())
            {
                return this.BadRequestError("You do not have permission to access this functionality!");
            }

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

            var allReports = new ReportAllViewModel()
            {
                AllReports = reportsAsViewModel,
                UserRole = this.User.Role
            };

            return this.View(allReports);
        }

    }
}
