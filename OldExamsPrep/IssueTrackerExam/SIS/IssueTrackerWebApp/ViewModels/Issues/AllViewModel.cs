namespace IssueTrackerWebApp.ViewModels.Issues
{
    using System;
    using System.Collections.Generic;

    public class AllViewModel
    {
        public ICollection<IssueViewModel> Issues { get; set; } = new List<IssueViewModel>();

        public Array StatusValues = new string[] {};

    }
}
