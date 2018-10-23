namespace IRunesWebApp.Services.Contracts
{
    using System.Collections.Generic;
    using Models;

    public interface IAlbumService
    {
        Album CreateAlbum(string name, string cover);

        IEnumerable<string> GetAllAlbums();

        Album GetAlbumById(string id);
    }
}
