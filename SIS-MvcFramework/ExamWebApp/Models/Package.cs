using System;

namespace ExamWebApp.Models
{
    public class Package
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public decimal Weight { get; set; }

        public string ShippingAddress { get; set; }

        public Status Status { get; set; }

        public DateTime EstimateDeliveryDate { get; set; }

        public int RecipientId { get; set; }

        public virtual User Recipient { get; set; }
    }
}
