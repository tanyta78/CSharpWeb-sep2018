namespace TorshiaWebApp.ViewModels
{
    public class ReportDetailsViewModel
    {
        public int Id { get; set; }

        public string TaskAffectedSectors { get; set; }
        
        public string TaskTitle { get; set; }

        public int TaskLevel { get; set; }

        public string Status { get; set; }

        public string TaskDescription { get; set; }

        public string TaskParticipants { get; set; }

        public string TaskDueDate { get; set; }

        public string ReportedOn { get; set; }

        public string Username { get; set; }
    }
}