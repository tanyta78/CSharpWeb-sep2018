namespace TorshiaWebApp.Models
{
    using System;
    using System.Collections.Generic;

    public class Task : BaseModel<int>
    {
        public string Title { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsReported { get; set; }

        public string Description { get; set; }

        public string Participants { get; set; }

        public virtual IEnumerable<TaskSector> AffectedSectors { get; set; } = new HashSet<TaskSector>();

    }
}