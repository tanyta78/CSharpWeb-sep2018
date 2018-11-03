namespace IssueTrackerWebApp.ViewModels.Issues
{
    public class IssueViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Priority { get; set; }

        public string Status { get; set; }

        public string CreationDate { get; set; }

        public string Author { get; set; }
    }
}
