namespace GameStoreWebApp.ViewModels.Games
{
    public class EditViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Thumbnail { get; set; }

        public int Size { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string Trailer { get; set; }
        
    }
}