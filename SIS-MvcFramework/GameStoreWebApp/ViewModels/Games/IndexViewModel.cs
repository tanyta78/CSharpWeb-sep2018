namespace GameStoreWebApp.ViewModels.Games
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public ICollection<GameViewModel> Games { get; set; }=new List<GameViewModel>();
    }
}
