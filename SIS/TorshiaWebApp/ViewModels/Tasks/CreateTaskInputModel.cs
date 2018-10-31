namespace TorshiaWebApp.ViewModels.Tasks
{
    using System;
    using System.Collections.Generic;
    using Models.Enums;

    public class CreateTaskInputModel
    {
        public string Title { get; set; }

        public DateTime DueDate { get; set; }

        public string Description { get; set; }

        public string Participants { get; set; }

        public ICollection<SectorType> AffectedSectors { get; set; } =new List<SectorType>();

        public string CustomersSector { get; set; }

        public string MarketingSector { get; set; }

        public string FinancesSector { get; set; }

        public string InternalSector { get; set; }

        public string ManagementSector { get; set; }
    }
}