namespace IssueTrackerWebApp.Controllers
{
    using System;

    public class EditIssueViewModel
    {
        public Array OptionStatusValues { get; set; }
        public Array OptionsPriorityValues { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
    }
}