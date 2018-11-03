namespace IssueTrackerWebApp.ViewModels.Issues
{
    using System;

    public class AddIssueViewModel
    {
        public Array StatusValues { get; set; } = new string[]{};
        public Array PriorityValues { get; set; } = new string[]{};
    }
}