namespace IRunesWebApp.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class AlbumService : IAlbumService
    {
        private readonly IRunesDbContext db;

        public AlbumService(IRunesDbContext db)
        {
            this.db = db;
        }

        //TODO:  change inline html with template
        public IEnumerable<string> GetAllAlbums()
        {
            return this.db.Albums.Select(a => $"<a href=\"/Albums/Details?id={a.Id.ToString().ToUpper()}\"/>{a.Name}</a> </br>");
        }

        public Album CreateAlbum(string name, string cover)
        {
            var album = new Album
            {
                Cover = cover,
                Name = name
            };

            this.db.Albums.Add(album);
            try
            {
                this.db.SaveChanges();
            }
            catch (Exception e)
            {
                //TODO: handle error
                Console.WriteLine(e.Message);
                return null;
            }

            return album;

        }

        public Album GetAlbumById(string id)
        {
            return this.db.Albums.Include(x => x.Tracks).ThenInclude(x => x.Track).FirstOrDefault(a => a.Id.ToString() == id);
        }

    }
}