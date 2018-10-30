namespace TorshiaWebApp.Services.Contracts
{
    using System;
    using Models;
    using Models.Enums;

    public class CreateReportInputModel
    {
        public Status Status { get; set; }

        public DateTime ReportedOn { get; set; }

        public Task Task { get; set; }

        public User Reporter { get; set; }
    }
}