namespace TorshiaWebApp.ViewModels
{
    using System.Collections.Generic;

    public class ReportAllViewModel
    {
        public IEnumerable<ReportViewModel> AllReports { get; set; } = new List<ReportViewModel>();
    }
}