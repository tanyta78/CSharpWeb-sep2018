namespace CasebookWebApp.ViewModels.Users
{
    using System.Collections.Generic;
    using Home;

    public class FriendsViewModel
    {
        public ICollection<UserViewModel> Friends { get; set; }
    }
}