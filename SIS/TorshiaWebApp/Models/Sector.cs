namespace TorshiaWebApp.Models
{
    using System.Collections.Generic;
    using Enums;

    public class Sector : BaseModel<int>
    {
        public SectorType SectorTask { get; set; }

        public virtual IEnumerable<TaskSector> AffectedSectors { get; set; } = new HashSet<TaskSector>();

    }
}