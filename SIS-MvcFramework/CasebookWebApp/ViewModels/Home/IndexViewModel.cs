namespace CasebookWebApp.ViewModels.Home
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public ICollection<UserViewModel> Users { get; set; }
    }
}