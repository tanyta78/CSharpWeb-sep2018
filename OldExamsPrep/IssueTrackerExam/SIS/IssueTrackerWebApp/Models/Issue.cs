namespace IssueTrackerWebApp.Models
{
    using System;

    public class Issue
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Priority Priority { get; set; }

        public Status Status { get; set; }

        public DateTime CreationDate { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
