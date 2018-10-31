namespace TorshiaWebApp.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Data;
    using Models;

    public class ReportService : IReportService
    {
        private readonly AppDbContext db;

        public ReportService(AppDbContext db)
        {
            this.db = db;
        }

        public Report CreateReport(CreateReportInputModel model)
        {
            var report = new Report()
            {
                ReportedOn = model.ReportedOn,
                ReporterId = model.Reporter.Id,
                TaskId = model.Task.Id,
                Status = model.Status
            };

            this.db.Reports.Add(report);

            try
            {
                this.db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

            return report;
        }

        public IEnumerable<Report> GetAllReports()
        {
            return this.db.Reports.ToList();
        }

        public Report GetReportById(int id)
        {
            var report = this.db.Reports.FirstOrDefault(t => t.Id == id);
            return report;
        }
    }
}