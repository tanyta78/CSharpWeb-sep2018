namespace TorshiaWebApp.ViewModels.Reports
{
    using System.Collections.Generic;

    public class ReportAllViewModel
    {
        public IEnumerable<ReportViewModel> AllReports { get; set; } = new List<ReportViewModel>();
        public string UserRole { get; set; }
    }
}