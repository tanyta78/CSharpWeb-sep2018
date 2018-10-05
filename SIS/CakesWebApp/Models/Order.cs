﻿namespace CakesWebApp.Models
{
    using System;
    using System.Collections.Generic;

    public class Order : BaseModel<int>
    {
        public DateTime DateOfCreation { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<OrderProduct> Products { get; set; } = new HashSet<OrderProduct>();
    }
}
