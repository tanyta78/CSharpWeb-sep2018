namespace TorshiaWebApp.ViewModels
{
    using System;

    public class TaskInfoViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Level { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsReported { get; set; }

        public string Description { get; set; }

        public string Participants { get; set; }

        public string AffectedSectors { get; set; }
    }
}