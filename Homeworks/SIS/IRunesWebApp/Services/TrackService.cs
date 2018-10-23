namespace IRunesWebApp.Services
{
    using System;
    using System.Linq;
    using Contracts;
    using Data;
    using Models;

    public class TrackService : ITrackService
    {
        private readonly IRunesDbContext db;

        public TrackService(IRunesDbContext db)
        {
            this.db = db;
        }

        public bool CreateTrack(Track track, string albumId)
        {
            this.db.Tracks.Add(track);
            var trackId = track.Id;
            var albumIdGuid = this.db.Albums.FirstOrDefault(a => a.Id.ToString() == albumId).Id;

            var albumTrack = new AlbumTrack
            {
                AlbumId = albumIdGuid,
                TrackId = trackId
            };
            this.db.AlbumsTracks.Add(albumTrack);
            try
            {
                this.db.SaveChanges();
            }
            catch (Exception e)
            {
                //TODO: handle error
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        public Track GetTrackById(string id)
        {
            return this.db.Tracks.FirstOrDefault(t => t.Id.ToString() == id);
        }
    }
}