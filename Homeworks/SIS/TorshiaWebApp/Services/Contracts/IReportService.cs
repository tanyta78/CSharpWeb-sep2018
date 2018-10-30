namespace TorshiaWebApp.Services.Contracts
{
    using System.Collections.Generic;
    using Models;

    public interface IReportService
    {
        Report GetReportById(int id);

        Report CreateReport(CreateReportInputModel model);

        IEnumerable<Report> GetAllReports();


    }
}