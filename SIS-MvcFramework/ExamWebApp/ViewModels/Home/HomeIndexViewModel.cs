using System.Collections.Generic;
using ExamWebApp.Models;

namespace ExamWebApp.ViewModels.Home
{
    public class HomeIndexViewModel
    {
        public ICollection<Package> Pending { get; set; }
        public ICollection<Package> Shipped { get; set; }
        public ICollection<Package> Delivered { get; set; }

    }
}
