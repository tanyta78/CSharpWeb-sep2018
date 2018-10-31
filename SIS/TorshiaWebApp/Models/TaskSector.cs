namespace TorshiaWebApp.Models
{
    using Enums;

    public class TaskSector : BaseModel<int>
    {
        public int TaskId { get; set; }

        public virtual Task Task { get; set; }

        public SectorType Sector { get; set; }

    }
}